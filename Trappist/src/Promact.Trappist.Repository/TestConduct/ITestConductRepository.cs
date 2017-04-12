using Promact.Trappist.DomainModel.Models.TestConduct;
using System.Threading.Tasks;

namespace Promact.Trappist.Repository.TestConduct
{
    public interface ITestConductRepository
    {
        /// <summary>
        /// This method used for register test attendee for the test.
        /// </summary>
        /// <param name="model">This model object contain test attendee credential which are first name, last name, email, roll number, contact number</param>
        /// <param name="magicString">This parameter contain test link</param>
        /// <returns> It will return true if test attendee successfully registered else it will return false.</returns>
        Task<bool> RegisterTestAttendeesAsync(TestAttendees model, string magicString);
    }
}