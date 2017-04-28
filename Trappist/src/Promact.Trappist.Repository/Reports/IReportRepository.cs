using Promact.Trappist.DomainModel.Models.TestConduct;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Promact.Trappist.Repository.Reports
{
    public interface IReportRepository
    {
        /// <summary>
        /// Method to get the details of all the Test-Attendees
        /// </summary>
        /// <param name="id">Id of the respective Test</param>
        /// <returns></returns>
        Task<ICollection<TestAttendees>> GetAllTestAttendeesAsync(int id);

        /// <summary>
        /// Method to set a candidate as Starred candidate
        /// </summary>
        /// <param name="id">Id of the candidate</param>
        /// <returns></returns>
        Task SetStarredCandidateAsync(int id);

        /// <summary>
        /// Method to set all candidates as starred candidate
        /// </summary>
        /// <param name="id">Id of the respective Test</param>
        /// <returns></returns>
        Task SetAllCandidateStarredAsync(int id, bool status);
    }
}
