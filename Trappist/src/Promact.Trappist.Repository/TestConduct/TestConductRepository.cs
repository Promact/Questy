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

        public async Task<InstructionAC> GetTestDetailsByLinkAsync(string testLink)
            {
            var testSettingsDetails = await _dbContext.Test.FirstOrDefaultAsync(x => x.Link == testLink);
            var currentTestId = testSettingsDetails.Id;
            var testQuestionDetails = await _dbContext.TestQuestion.Where(x => x.TestId == currentTestId).ToListAsync();
            var totalNumberOfQuestions = testQuestionDetails.Count();
            var testCategoryDetails = await _dbContext.TestCategory.Where(x => x.TestId == currentTestId).ToListAsync();
            var testCategoryNameList = new List<string>();
            foreach (var testCategory in testCategoryDetails)
            {
                var categoryDetails = await _dbContext.Category.FirstOrDefaultAsync(x => x.Id == testCategory.CategoryId);
                var categoryName = categoryDetails.CategoryName;
                testCategoryNameList.Add(categoryName);
            }
            var instruction = new InstructionAC()
            {
                Duration = testSettingsDetails.Duration,
                WarningTime = testSettingsDetails.WarningTime,
                CorrectMarks = testSettingsDetails.CorrectMarks,
                IncorrectMarks = testSettingsDetails.IncorrectMarks,
                TotalNumberOfQuestions = totalNumberOfQuestions,
                CategoryNameList = testCategoryNameList
            };
            return instruction;
        }

        public async Task<bool> IsTestLinkExistAsync(string magicString)
        {
            var isTestLinkExist = await (_dbContext.Test.AnyAsync(x => (x.Link == magicString)));
            return isTestLinkExist;
        }
        #endregion
    }
}        