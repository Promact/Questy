using Promact.Trappist.DomainModel.Models.Test;
using Promact.Trappist.DomainModel.Models.TestConduct;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Promact.Trappist.Repository.Report
{
    public interface IReportRepository
    {
        /// <summary>
        /// Gets all the test attendees present in a test
        /// </summary>
        /// <param name="testId">Id of the test whose attendees are to be fetched</param>
        /// <returns>A list of test attendees present in the test</returns>
        IEnumerable GetAllTestAttendees(int testId);

        /// <summary>
        /// Gets thedetails of the test attendee along with his marks and test logs
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
    }
}
