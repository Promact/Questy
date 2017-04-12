using System.Threading.Tasks;
using Promact.Trappist.DomainModel.Models.TestConduct;
using Promact.Trappist.DomainModel.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Promact.Trappist.Repository.TestConduct
{
    public class TestConductRepository : ITestConductRepository
    {
        #region Private Variables
        #region Dependencies
        private readonly TrappistDbContext _dbContext;
        #endregion
        #endregion

        #region Constructor
        public TestConductRepository(TrappistDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region Public Method
        public async Task<bool> RegisterTestAttendeesAsync(TestAttendees model, string magicString)
        {
            if (!await IsTestAttendeeExistAsync(model, magicString))
            {
                await _dbContext.TestAttendees.AddAsync(model);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        /// <summary>
        /// This method used for check test attendee already exist for this test.
        /// </summary>
        /// <param name="model">This model object contain test attendee credential which are first name,last name,email,roll number,contact number</param>
        /// <param name="magicString">This parameter contain test link</param>
        /// <returns>If test attendee exist then return false else return true.</returns>
        private async Task<bool> IsTestAttendeeExistAsync(TestAttendees model, string magicString)
        {
            //TO DO:Handling of wrong magic string is required to implement.
            var testObject = (await _dbContext.Test.FirstOrDefaultAsync(x => (x.Link == magicString)));
            if (testObject != null)
            {
                model.TestId = testObject.Id;
                var isTestAttendeeExist = await (_dbContext.TestAttendees.AnyAsync(x => (x.Email == model.Email && x.TestId == model.TestId && x.RollNumber == model.RollNumber)));
                return isTestAttendeeExist;
            }
            return false;
        }
        #endregion
    }
}