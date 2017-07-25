using Promact.Trappist.DomainModel.ApplicationClasses.Test;
using Promact.Trappist.DomainModel.ApplicationClasses.TestConduct;
using Promact.Trappist.DomainModel.Enum;
using Promact.Trappist.DomainModel.Models.TestConduct;
using System;
using System.Collections.Generic;
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

        /// <summary>
        /// This method used to check magicString exist or not for any test, and whether the same test is paused or not
        /// </summary>
        /// <param name="magicString">It contain random string which uniquely identifies test</param>
        /// <returns>If test link exist then return true else return false</returns>
        Task<bool> IsTestLinkExistForTestConductionAsync(string magicString);

        /// <summary>
        /// This method is used to get all the instruction details related to a test before starting it
        /// </summary>
        /// <param name="testLink">link to conduct a particular test</param>
        /// <returns>instructions for a particular test</returns>
        Task<TestInstructionsAC> GetTestInstructionsAsync(string testLink);
        
        /// <summary>
        /// Store answers as Key-Value pair in the Database
        /// </summary>
        /// <param name="attendeeId">Id of Test Attendee</param>
        Task AddAnswerAsync(int attendeeId, TestAnswerAC answer);

        /// <summary>
        /// Checks if the question is already answered
        /// </summary>
        /// <param name="attendeeId">Id of Test Attendee</param>
        /// <param name="answer">TestAnswerAC object</param>
        /// <returns>True if question is already answered, false otherwise.</returns>
        Task<bool> IsAnswerValidAsync(int attendeeId, TestAnswerAC answer);

        /// <summary>
        /// Get answers from the Database
        /// </summary>
        /// <param name="attendeeId">Id of Test Attendee</param>
        /// <returns>Answers as a string type</returns>
        Task<ICollection<TestAnswerAC>> GetAnswerAsync(int attendeeId);

        /// <summary>
        /// Gets Test Attendee information by Id
        /// </summary>
        /// <param name="testId">Id of Test</param>
        /// <returns>TestAttendee object</returns>
        Task<TestAttendees> GetTestAttendeeByIdAsync(int attendeeId);

        /// <summary>
        /// Checks if Test Attendee exist
        /// </summary>
        /// <param name="attendeeId">Id of Test Attendee</param>
        /// <returns>Boolean value true if Test Attendee exist else false</returns>
        Task<bool> IsTestAttendeeExistByIdAsync(int attendeeId);

        /// <summary>
        /// Set the time elapsed from start of Test
        /// </summary>
        /// <param name="attendeeId">Id of Attendee</param>
        Task SetElapsedTimeAsync(int attendeeId);

        /// <summary>
        /// Get the time elapsed from start of Test
        /// </summary>
        /// <param name="attendeeId">Id of Attendee</param>
        /// <returns>DateTime objects</returns>
        Task<double> GetElapsedTimeAsync(int attendeeId);

        /// <summary>
        /// Sets the Test Status of the Attendee
        /// </summary>
        /// <param name="attendeeId">Id of the Attendee</param>
        /// <param name="testStatus">TestStatus enum</param>
        Task SetAttendeeTestStatusAsync(int attendeeId, TestStatus testStatus);

        /// <summary>
        /// Gets the Test Status of the Attendee
        /// </summary>
        /// <param name="attendeeId">Id of the Attendee</param>
        /// <return>TestStatus object</return>
        Task<TestStatus> GetAttendeeTestStatusAsync(int attendeeId);

        /// <summary>
        /// Checks if the Test is in valid date window.
        /// </summary>
        /// <param name="testLink">link to conduct a particular test</param>
        /// <returns>
        /// True if current date lies between Test's start date and end date.
        /// False otherwise.
        /// </returns>
        Task<bool> IsTestInValidDateWindow(string testLink);
    }
}