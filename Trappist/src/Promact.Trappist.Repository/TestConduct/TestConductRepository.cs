using System.Threading.Tasks;
using Promact.Trappist.DomainModel.Models.TestConduct;
using Promact.Trappist.DomainModel.DbContext;
using Microsoft.EntityFrameworkCore;
using Promact.Trappist.DomainModel.ApplicationClasses.TestConduct;
using System.Collections.Generic;
using System.Linq;

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
        public async Task RegisterTestAttendeesAsync(TestAttendees testAttendee, string magicString)
        {
            await _dbContext.TestAttendees.AddAsync(testAttendee);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> IsTestAttendeeExistAsync(TestAttendees testAttendee, string magicString)
        {
            var testObject = (await _dbContext.Test.FirstOrDefaultAsync(x => (x.Link == magicString)));
            if (testObject != null)
            {
                testAttendee.TestId = testObject.Id;
                var isTestAttendeeExist = await (_dbContext.TestAttendees.AnyAsync(x => (x.Email == testAttendee.Email && x.TestId == testAttendee.TestId && x.RollNumber == testAttendee.RollNumber)));
                return isTestAttendeeExist;
            }
            return false;
        }

        public async Task<TestInstructionsAC> GetTestInstructionsAsync(string testLink)
        {
            var testDeatilsObj = await _dbContext.Test.Where(x => x.Link == testLink)
                .Include(x => x.TestQuestion)
                .Include(x => x.TestCategory)
                .ThenInclude(x => x.Category).ToListAsync();
            if (testDeatilsObj != null)
            {
                var testInstructionDetails = testDeatilsObj.First();
                var totalNumberOfQuestions = testInstructionDetails.TestQuestion.Count();
                var testCategoryNameList = testInstructionDetails.TestCategory.Select(x => x.Category.CategoryName).ToList();
                var testInstructions = new TestInstructionsAC()
                {
                    Duration = testInstructionDetails.Duration,
                    WarningTime = testInstructionDetails.WarningTime,
                    CorrectMarks = testInstructionDetails.CorrectMarks,
                    IncorrectMarks = testInstructionDetails.IncorrectMarks,
                    TotalNumberOfQuestions = totalNumberOfQuestions,
                    CategoryNameList = testCategoryNameList
                };
                return testInstructions;
            }
            return null;
        }

        public async Task<bool> IsTestLinkExistAsync(string magicString)
        {
            var isTestLinkExist = await (_dbContext.Test.AnyAsync(x => (x.Link == magicString)));
            return isTestLinkExist;
        }
        #endregion
    }
}