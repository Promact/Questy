using Microsoft.AspNetCore.SignalR;
using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.Models.TestConduct;
using Promact.Trappist.Repository.TestConduct;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        private const string ADMIN_GROUP = "__admin";
        public TrappistHub(ITestConductRepository testConductRepository, TrappistDbContext dbContext)
        {
            _dbContext = dbContext;
            _testConductRepository = testConductRepository;
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            var currentDate = DateTime.Now;
            Console.Write(currentDate);
            string connectionId = Context.ConnectionId;
            Attendee attendee;
            Attendees.TryGetValue(connectionId, out attendee);
            var totalSeconds = (currentDate - attendee.StartDate).TotalSeconds;
            var sec = (long)totalSeconds;
            await _testConductRepository.SetElapsedTimeAsync(attendee.AttendeeId, sec, true);
            await base.OnDisconnectedAsync(exception);
        }

        public void RegisterAttendee(int id)
        {
            var startDate = DateTime.Now;
            string connectionId = Context.ConnectionId;
            var attendee = new Attendee()
            {
                AttendeeId = id,
                StartDate = startDate,
                ConnectionIds = new HashSet<string>()
            };
            Attendees.AddOrUpdate(connectionId, attendee, (key, oldvalue) => attendee);
            lock (attendee.ConnectionIds)
            {
                attendee.ConnectionIds.Add(connectionId);
            }

        }

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

        public Task GetExpectedEndTime(long seconds)
        {
            return Clients.Group(ADMIN_GROUP).InvokeAsync("");
        }

        public Task JoinAdminGroup()
        {
            return Groups.AddAsync(Context.ConnectionId, ADMIN_GROUP);
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
