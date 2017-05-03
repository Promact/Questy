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
        /// <param name="id">Id of the respective test</param>
        /// <returns></returns>
        Task<IEnumerable<TestAttendees>> GetAllTestAttendeesAsync(int id);

        /// <summary>
        /// Method to set a candidate as Starred candidate
        /// </summary>
        /// <param name="id">Id of the candidate</param>
        /// <returns></returns>
        Task SetStarredCandidateAsync(int id);

        /// <summary>
        /// Method to set a list of candidates as starred candidate.
        /// </summary>
        /// <param name="id">Id if the test</param>
        /// <param name="status">Star status of the student</param>
        /// <param name="idList">List of id of test attendees</param>
        /// <returns></returns>
        Task SetAllCandidateStarredAsync(bool status,  int selectedTestStatus, string SearchString);
    }
}
