using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Promact.Trappist.DomainModel.ApplicationClasses.TestConduct;
using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.Models.TestConduct;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

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
                    BrowserTolerance = testInstructionDetails.BrowserTolerance,
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

        public async Task AddAnswerAsync(int attendeeId, string answers)
        {
            if (await _dbContext.AttendeeAnswers.AnyAsync(x => x.Id == attendeeId))
            {
                var answersToUpdate = await _dbContext.AttendeeAnswers.FindAsync(attendeeId);
                answersToUpdate.Answers = answers;
            }
            else
            {
                var attendeeAnswers = new AttendeeAnswers();
                attendeeAnswers.Id = attendeeId;
                attendeeAnswers.Answers = answers;
                await _dbContext.AddAsync(attendeeAnswers);
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ICollection<TestAnswerAC>> GetAnswerAsync(int attendeeId)
        {
            var attendee = await _dbContext.AttendeeAnswers.FindAsync(attendeeId);

            if (attendee == null)
                return null;

            var deserializedAttendeeAnswers = JsonConvert.DeserializeObject<ICollection<TestAnswerAC>>(attendee.Answers);
            return deserializedAttendeeAnswers;
        }

        public async Task<TestAttendees> GetTestAttendeeByIdAsync(int attendeeId)
        {
            return await _dbContext.TestAttendees.SingleAsync(x => x.Id == attendeeId);
        }

        public async Task<bool> IsTestAttendeeExistByIdAsync(int attendeeId)
        {
            return await _dbContext.TestAttendees.AnyAsync(x => x.Id == attendeeId);
        }

        public async Task SetElapsedTimeAsync(int attendeeId)
        {
            var attendee = await _dbContext.AttendeeAnswers.FirstOrDefaultAsync(x => x.Id == attendeeId);
            if (attendee != null)
            {
                var createdTime = attendee.CreatedDateTime;
                var currentTime = DateTime.UtcNow;
                var timeSpan = currentTime.Subtract(createdTime);
                attendee.TimeElapsed = timeSpan.TotalMinutes;
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                await AddAnswerAsync(attendeeId, null);
            }
        }

        public async Task<double> GetElapsedTimeAsync(int attendeeId)
        {
            return await _dbContext.AttendeeAnswers.Where(x => x.Id == attendeeId).Select(x => x.TimeElapsed).FirstOrDefaultAsync(); 
        }
        #endregion
    }
}