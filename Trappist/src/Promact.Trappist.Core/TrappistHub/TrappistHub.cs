﻿using Microsoft.AspNetCore.SignalR;
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
        public bool IsAttendeeReset { get; set; }
    }


    public class TrappistHub : Hub
    {
        private static readonly ConcurrentDictionary<string, Attendee> Attendees = new();
        private readonly ITestConductRepository _testConductRepository;
        private const string ADMIN_GROUP = "__admin";
        public TrappistHub(ITestConductRepository testConductRepository)
        {
            _testConductRepository = testConductRepository;
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string connectionId = Context.ConnectionId;
            Attendees.TryGetValue(connectionId, out Attendee attendee);
            if (attendee.IsAttendeeReset)
            {
                var currentDate = DateTime.UtcNow;
                Console.Write(currentDate);                
                var totalSeconds = (currentDate - attendee.StartDate).TotalSeconds;
                var sec = (long)totalSeconds;
                await _testConductRepository.SetElapsedTimeAsync(attendee.AttendeeId, sec, true);
            }

            await base.OnDisconnectedAsync(exception);
        }

        public void RegisterAttendee(int id)
        {
            var startDate = DateTime.UtcNow;
            string connectionId = Context.ConnectionId;
            var attendee = new Attendee()
            {
                AttendeeId = id,
                StartDate = startDate,
                ConnectionIds = new HashSet<string>(),
                IsAttendeeReset = true
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
            string connectionId = Context.ConnectionId;
            Attendees.TryGetValue(connectionId, out Attendee attendee);
            if(attendee != null) attendee.IsAttendeeReset = false;

            return Clients.All.SendAsync("getReport", testAttendee);
        }

        /// <summary>
        /// This method gets the id of the candidate who requests for resume test and pass this id to getAttendeeIdWhoRequestedForResumeTest method in test report page
        /// </summary>
        /// <param name="attendeeId"></param>
        /// <returns>invokes the method getAttendeeIdWhoRequestedForResumeTest and pass the id in test report page</returns>
        public Task SendCandidateIdWhoRequestedForResumeTest(int attendeeId)
        {
            return Clients.All.SendAsync("getAttendeeIdWhoRequestedForResumeTest", attendeeId);
        }

        /// <summary>
        /// Gets the expected test end time
        /// </summary>
        /// <param name="duration">Test duration</param>
        /// <param name="testId">Test Id</param>
        /// <returns>Invokes setEstimatedEndTime method in client side</returns>
        public async Task<Task> GetExpectedEndTime(double duration, int testId)
        {
            var expectedTimeString = await _testConductRepository.GetExpectedTestEndTime(duration, testId);
            return Clients.Group(ADMIN_GROUP).SendAsync("setEstimatedEndTime", expectedTimeString);
        }

        public Task JoinAdminGroup()
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, ADMIN_GROUP);
        }
    }
}
