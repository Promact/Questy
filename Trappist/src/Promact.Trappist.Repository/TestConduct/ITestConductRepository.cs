﻿using Promact.Trappist.DomainModel.Models.TestConduct;
using System.Threading.Tasks;

namespace Promact.Trappist.Repository.TestConduct
{
    public interface ITestConductRepository
    {
        /// <summary>
        /// This method used for register test attendee for the test.
        /// </summary>
        /// <param name="testAttendee">This model object contain test attendee credential which are first name, last name, email, roll number, contact number</param>
        /// <param name="magicString">This parameter contain test link</param>
        /// <returns></returns>
        Task RegisterTestAttendeesAsync(TestAttendees testAttendee, string magicString);

        /// <summary>
        /// This method used for check test attendee already exist for this test.
        /// </summary>
        /// <param name="testAttendee">This model object contain test attendee credential which are first name,last name,email,roll number,contact number</param>
        /// <param name="magicString">This parameter contain test link</param>
        /// <returns>If test attendee exist then return true else return false.</returns>
        Task<bool> IsTestAttendeeExistAsync(TestAttendees testAttendee, string magicString);
    }
}