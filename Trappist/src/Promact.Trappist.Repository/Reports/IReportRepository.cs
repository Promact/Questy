using Promact.Trappist.DomainModel.ApplicationClasses.Reports;
using Promact.Trappist.DomainModel.Models.Report;
using Promact.Trappist.DomainModel.Models.Test;
using Promact.Trappist.DomainModel.Models.TestConduct;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Promact.Trappist.Repository.Reports
{
    public interface IReportRepository
    {
        /// <summary>
        /// Method to get the details of all the Test-Attendees of the respective test
        /// </summary>
        /// <param name="testId">Id of the respective test</param>
        /// <returns>All test attendees of that respective test</returns>
        Task<IEnumerable<TestAttendees>> GetAllTestAttendeesAsync(int id);

        /// <summary>
        /// Method to set a candidate as Starred candidate
        /// </summary>
        /// <param name="id">Id of the candidate</param>
        /// <returns>Attendee Id</returns>
        Task SetStarredCandidateAsync(int id);

        /// <summary>
        /// Method to set all candidates with matching criteria as starred candidate
        /// </summary>
        /// <param name="status">Star status of the candidates</param>
        /// <param name="selectedTestStatus">Test end status of the candidates</param>
        /// <param name="searchString">The search string provided by the user</param>
        /// <returns>Star status of the candidates</returns>
        Task SetAllCandidateStarredAsync(bool status, int selectedTestStatus, string SearchString);

        /// <summary>
        /// Method to check whether a candidate exist or not
        /// </summary>
        /// <param name="attendeeId">Id of the candidate</param>
        /// <returns>returns boolean type result depending on whether the candidate exist or not</returns>
        Task<bool> IsCandidateExistAsync(int attendeeId);

        /// <summary>
        ///Method to get test name 
        /// </summary>
        /// <param name="id">Id of the test</param>
        /// <returns>Returns the name of the test matching with the Id</returns>
        Task<Test> GetTestNameAsync(int id);

        /// <summary>
        /// Gets the details of the test attendee along with his marks and test logs
        /// </summary>
        /// <param name="id"> Id of the test attendee</param>
        /// <returns> An object of test attendee</returns>
        Task<TestAttendees> GetTestAttendeeDetailsByIdAsync(int id);

        /// <summary>
        /// Gets all the questions present in a test
        /// </summary>
        /// <param name="testId"> Id of the test qhose questions are to be fetched</param>
        /// <returns>A list of all the questions present in the test</returns>
        Task<List<TestQuestion>> GetTestQuestions(int testId);

        /// <summary>
        /// Gets all the answers given by a test attendee
        /// </summary>
        /// <param name="testAttendeeId">Id of the test attendee</param>
        /// <returns>A list of all the answers given by the test attendee</returns>
        Task<List<TestAnswers>> GetTestAttendeeAnswers(int testAttendeeId);

        /// <summary>
        /// Calculates the percentile of selected test attendee
        /// </summary>
        /// <param name="testAttendeeId">testAttendee contains the id of the test attendee from the route</param>
        /// <returns>The calculated percentile value of the selected test attendee</returns>
        Task<double> CalculatePercentileAsync(int testAttendeeId);

        /// <summary>
        /// Calculate all marks details of all attendee of a particular test
        /// </summary>
        /// <param name="testId">Id of a test</param>
        /// <returns></returns>
        Task<List<ReportQuestionsCountAC>> GetAllAttendeeMarksDetailsAsync(int testId);
        /// <summary>
        /// set test status type to allCandidate
        /// </summary>
        /// <param name="attendee"></param>
        /// <returns></returns>
        Task<TestAttendees> SetTestStatusAsync(TestAttendees attendee);

        /// <summary>
        /// set isTestPausedUnWillingly to true
        /// </summary>
        /// <param name="attendeeId"></param>
        /// <returns></returns>
        Task SetWindowCloseAsync(int attendeeId, bool isTestResume);

        /// <summary>
        /// gets the value of isTestPausedUnWillingly
        /// </summary>
        /// <param name="attendeeId"></param>
        /// <returns></returns>
        Task<bool> GetWindowCloseAsync(int attendeeId);
    }
}
