using Microsoft.AspNetCore.SignalR;
using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.Models.TestConduct;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.Utility.Constants;
using System.Linq;
using Promact.Trappist.Repository.TestConduct;
using System.Collections.Concurrent;
using Promact.Trappist.DomainModel.Models.TestLogs;
using Microsoft.EntityFrameworkCore;

namespace Promact.Trappist.Core.TrappistHub
{
    public class Attendee
    {
        public int AttendeeId { get; set; }
        public DateTime StartDate { get; set; }
        public HashSet<string> ConnectionIds { get; set; }
    }


    public class TrappistHub : Hub
    {
        private static readonly ConcurrentDictionary<string, Attendee> Attendees
       = new ConcurrentDictionary<string, Attendee>();
        private readonly ITestConductRepository _testConductRepository;
        private readonly TrappistDbContext _dbContext;
        public TrappistHub(ITestConductRepository testConductRepository, TrappistDbContext dbContext)
        {
            _dbContext = dbContext;
            _testConductRepository = testConductRepository;
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            var currentDate = DateTime.UtcNow;
            string connectionId = Context.ConnectionId;
            Attendee attendee;
            Attendees.TryGetValue(connectionId, out attendee);
            var totalSeconds = (currentDate - attendee.StartDate).TotalSeconds;
            var sec = (long)totalSeconds;
            await _testConductRepository.SetElapsedTimeAsync(attendee.AttendeeId, sec);
            await base.OnDisconnectedAsync(exception);
        }

        public void RegisterAttendee(int id)
        {
            string connectionId = Context.ConnectionId;
            var attendee = Attendees.GetOrAdd(connectionId, _ => new Attendee
            {
                AttendeeId = id,
                StartDate = DateTime.UtcNow,
                ConnectionIds = new HashSet<string>()
            });
            lock (attendee.ConnectionIds)
            {
                attendee.ConnectionIds.Add(connectionId);
            }

        }
        //public async Task AddTestLogs(int id)
        //{
        //    string connectionId = Context.ConnectionId;
        //    Attendee testAttendee;
        //    Attendees.TryGetValue(connectionId, out testAttendee);
        //    var startDate = DateTime.UtcNow;
        //    var attendee = new Attendee();
        //    attendee.StartDate = startDate;
        //    attendee.AttendeeId = testAttendee.AttendeeId;
        //    attendee.ConnectionIds = testAttendee.ConnectionIds;
        //    Attendees.TryUpdate(connectionId , attendee, testAttendee);
        //}

        /// <summary>
        /// This method gets the generated report of the candidate and then sends back to the report display page 
        /// </summary>
        /// <param name="testAttendee"></param>
        /// <returns>Sends the testAttendee object to getReport method in client side</returns>
        public Task SendReport(TestAttendees testAttendee)
        {
            return Clients.All.InvokeAsync("getReport", testAttendee);
        }

        /// <summary>
        /// This method gets the id of the candidate who requests for resume test and pass this id to getAttendeeIdWhoRequestedForResumeTest method in test report page
        /// </summary>
        /// <param name="attendeeId"></param>
        /// <returns>invokes the method getAttendeeIdWhoRequestedForResumeTest and pass the id in test report page</returns>
        public Task SendCandidateIdWhoRequestedForResumeTest(int attendeeId)
        {
            return Clients.All.InvokeAsync("getAttendeeIdWhoRequestedForResumeTest", attendeeId);
        }
    }

    //public class HubController : Controller
    //{
    //    private readonly IStringConstants _stringConstants;
    //    private readonly TrappistDbContext _dbContext;

    //    public HubController(IStringConstants stringConstants,TrappistDbContext dbContext)
    //    {
    //        _stringConstants = stringConstants;
    //        _dbContext = dbContext;
    //    }

    //    public void SaveData()
    //    {
    //        HttpContext.Session.GetInt32(_stringConstants.AttendeeIdSessionKey);
    //        _dbContext.TestLogs.
    //    }

    //}
}
