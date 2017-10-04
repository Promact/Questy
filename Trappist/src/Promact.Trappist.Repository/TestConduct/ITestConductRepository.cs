using Promact.Trappist.DomainModel.ApplicationClasses.CodeSnippet;
using Promact.Trappist.DomainModel.ApplicationClasses.TestConduct;
using Promact.Trappist.DomainModel.Enum;
using Promact.Trappist.DomainModel.Models.TestConduct;
using Promact.Trappist.DomainModel.Models.TestLogs;
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
        Task<bool> IsTestLinkExistForTestConductionAsync(string magicString, string userIp);

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
        /// Gets Test Attendee using email and rollno
        /// </summary>
        /// <param name="email">Email of Test Attendee</param>
        /// <param name="rollno">Roll No of Test Attendee</param>
        /// <returns>TestAttendee object</returns>
        Task<TestAttendees> GetTestAttendeeByEmailIdAndRollNo(string email, string rollno, int testId);

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
        /// <param name="seconds">Elapsed seconds</param>
        Task SetElapsedTimeAsync(int attendeeId, long seconds);

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
        Task<TestAttendees> SetAttendeeTestStatusAsync(int attendeeId, TestStatus testStatus);

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
        Task<bool> IsTestInValidDateWindow(string testLink, bool isPreview);

        /// <summary>
        /// Add values to specific fields of Test Logs Model
        /// </summary>
        /// <param name="attendeeId">Id of a particular Test Attendee</param>
        /// <param name="testLogs">It is an object of TestLogs type</param>
        /// <returns></returns>
        Task<bool> AddTestLogsAsync(int attendeeId, bool isCloseWindow, bool isTestResume);

        /// <summary>
        /// Evaluate and Access code snippet
        /// </summary>
        /// <param name="attendeeId">Id of Test Attendee</param>
        /// <param name="code">Code Object</param>
        /// <returns>True if all test case is passed else false</returns>
        Task<CodeResponse> ExecuteCodeSnippetAsync(int attendeeId, TestAnswerAC testAnswer);

        /// <summary>
        /// Calculates the total number of questions for a Test
        /// </summary>
        /// <param name="testLink">Contains the link of the Test</param>
        /// <returns>The total number of questions of that Test</returns>
        Task<int> GetTestSummaryDetailsAsync(string testLink);

        Task<List<TestLogs>> GetTestLogsAsync();

        /// <summary>
        /// Sets the attendee browser tolerance count 
        /// </summary>
        /// <param name="attendeeId">Contains the attendee Id from the route</param>
        /// <param name="attendeeBrowserToleranceCount">Contains the attendee browser tolerance count from the route</param>
        /// <returns>The browser tolerance count left for an attendee</returns>
        Task SetAttendeeBrowserToleranceValueAsync(int attendeeId, int attendeeBrowserToleranceCount);
    }
}