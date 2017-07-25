using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Promact.Trappist.DomainModel.ApplicationClasses.TestConduct;
using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.Models.TestConduct;
using Promact.Trappist.Repository.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Trappist.Repository.TestConduct
{
    public class TestConductRepository : ITestConductRepository
    {
        #region Private Variables
        #region Dependencies
        private readonly TrappistDbContext _dbContext;
        private readonly ITestsRepository _testRepository;
        #endregion
        #endregion

        #region Constructor
        public TestConductRepository(TrappistDbContext dbContext, ITestsRepository testRepository)
        {
            _dbContext = dbContext;
            _testRepository = testRepository;
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
            var testObject = await _dbContext.Test.Where(x => x.Link == testLink)
                    .Include(x => x.TestQuestion).ToListAsync();
            if (testObject.Any())
            {
                var testInstructionsDetails = testObject.First();
                var totalNumberOfQuestions = testInstructionsDetails.TestQuestion.Count();
                var testCategoryANameList = new List<string>();
                var testByIdObj = await _testRepository.GetTestByIdAsync(testInstructionsDetails.Id, testInstructionsDetails.CreatedByUserId);
                var categoryAcList = testByIdObj.CategoryAcList;
                foreach (var category in categoryAcList)
                {
                    if (category.NumberOfSelectedQuestion != 0 && category.IsSelect)
                    {
                        testCategoryANameList.Add(category.CategoryName);
                    }
                }
                var testInstructions = new TestInstructionsAC()
                {
                    Duration = testInstructionsDetails.Duration,
                    BrowserTolerance = testInstructionsDetails.BrowserTolerance,
                    CorrectMarks = testInstructionsDetails.CorrectMarks,
                    IncorrectMarks = testInstructionsDetails.IncorrectMarks,
                    TotalNumberOfQuestions = totalNumberOfQuestions,
                    CategoryNameList = testCategoryANameList
                };
                return testInstructions;
            }
            return null;
        }

        public async Task<bool> IsTestLinkExistForTestConductionAsync(string magicString)
        {
            return await (_dbContext.Test.AnyAsync(x => x.Link == magicString && !x.IsPaused));
        }

        public async Task AddAnswerAsync(int attendeeId, TestAnswerAC answer)
        {
            if (await _dbContext.AttendeeAnswers.AnyAsync(x => x.Id == attendeeId))
            {
                var attendeeAnswer = await _dbContext.AttendeeAnswers.FindAsync(attendeeId);
                var deserializedAnswer = new List<TestAnswerAC>();

                if (attendeeAnswer.Answers != null)
                {
                    deserializedAnswer = JsonConvert.DeserializeObject<TestAnswerAC[]>(attendeeAnswer.Answers).ToList();
                }

                //Remove answer if already exist
                var answerToUpdate = deserializedAnswer.SingleOrDefault(x => x.QuestionId == answer.QuestionId);
                if (answerToUpdate != null)
                {
                    deserializedAnswer.Remove(answerToUpdate);
                }
                
                //Add answer
                deserializedAnswer.Add(answer);
                var serializedAnswer = JsonConvert.SerializeObject(deserializedAnswer);
                attendeeAnswer.Answers = serializedAnswer;
                _dbContext.AttendeeAnswers.Update(attendeeAnswer);
            }
            else
            {
                var attendeeAnswers = new AttendeeAnswers();
                attendeeAnswers.Id = attendeeId;

                if (answer != null)
                {
                    var testAnswerArray = new List<TestAnswerAC>();
                    testAnswerArray.Add(answer);
                    attendeeAnswers.Answers = JsonConvert.SerializeObject(testAnswerArray);
                }
                await _dbContext.AddAsync(attendeeAnswers);
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> IsAnswerValidAsync(int attendeeId, TestAnswerAC answer)
        {
            var attendeeAnswer = await _dbContext.AttendeeAnswers.FindAsync(attendeeId);
            if(attendeeAnswer.Answers != null)
            {
                var deserializedAnswer = JsonConvert.DeserializeObject<TestAnswerAC[]>(attendeeAnswer.Answers);
                if(deserializedAnswer[0] != null)
                {
                    return !(deserializedAnswer.Where(x => x.QuestionId == answer.QuestionId && x.QuestionStatus == QuestionStatus.answered).Count() > 0);
                }
            }
            return true;
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
            var attendee = await _dbContext.AttendeeAnswers.FindAsync(attendeeId);
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
            var attendeeAnswer = await _dbContext.AttendeeAnswers.FindAsync(attendeeId);
            return attendeeAnswer.TimeElapsed;
        }

        public async Task SetAttendeeTestStatusAsync(int attendeeId, TestStatus testStatus)
        {
            var report = await _dbContext.Report.OrderBy(x => x.TestAttendeeId).FirstOrDefaultAsync(x => x.TestAttendeeId == attendeeId);
            if (report == null)
            {
                report = new Report();
                report.TestAttendeeId = attendeeId;
            }
            report.TestStatus = testStatus;
            await _dbContext.Report.AddAsync(report);
            await _dbContext.SaveChangesAsync();

            if(testStatus != TestStatus.AllCandidates)
            {
                //Begin transformation
                await TransformAttendeeAnswer(attendeeId);
            }
        }

        public async Task<TestStatus> GetAttendeeTestStatusAsync(int attendeeId)
        {
            var report = await _dbContext.Report.OrderBy(x => x.TestAttendeeId).FirstOrDefaultAsync(x => x.TestAttendeeId == attendeeId);
            if (report == null)
                return TestStatus.AllCandidates;

            return report.TestStatus;
        }

        public async Task<bool> IsTestInValidDateWindow(string testLink)
        {
            var currentDate = DateTime.UtcNow;
            var startTime = await _dbContext.Test.Where(x => x.Link == testLink).Select(x => x.StartDate).SingleAsync();
            var endTime = await _dbContext.Test.Where(x => x.Link == testLink).Select(x => x.EndDate).SingleAsync();

            if(currentDate.CompareTo(startTime) >= 0 && currentDate.CompareTo(endTime) <= 0)
            {
                return true;
            }

            return false;
        }
        #endregion

        #region Private Method

        /// <summary>
        /// Transform Attendee's JSON formatted Answer stored in AttendeeAnswers table
        /// to reliable TestAnswer and TestConduct tables.
        /// After completion of transformation Attendee's record from AttendeeAnswer is removed.
        /// </summary>
        /// <param name="attendeeId">Id of Test Attendee</param>
        private async Task TransformAttendeeAnswer(int attendeeId)
        {
            var attendeeAnswer = await _dbContext.AttendeeAnswers.SingleOrDefaultAsync(x => x.Id == attendeeId);

            if(attendeeAnswer != null)
            {
                if(attendeeAnswer.Answers != null)
                {
                    var deserializedAnswer = JsonConvert.DeserializeObject<TestAnswerAC[]>(attendeeAnswer.Answers).ToList();
                    
                    foreach(var answer in deserializedAnswer)
                    {
                        var testAnswers = new TestAnswers();
                        //Adding attempted Question to TestConduct table
                        var testConduct = new DomainModel.Models.TestConduct.TestConduct()
                        {
                            QuestionId = answer.QuestionId,
                            QuestionStatus = answer.QuestionStatus,
                            TestAttendeeId = attendeeId
                        };
                        await _dbContext.TestConduct.AddAsync(testConduct);
                        await _dbContext.SaveChangesAsync();

                        //Adding answer to TestAnswer Table
                        if (answer.OptionChoice.Count() > 0)
                        {
                            //A question can have multiple answer
                            foreach(var option in answer.OptionChoice)
                            {
                                testAnswers = new TestAnswers()
                                {
                                    AnsweredOption = option,
                                    TestConduct = testConduct
                                };
                                await _dbContext.TestAnswers.AddAsync(testAnswers);
                                await _dbContext.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            //Save answer for code snippet question
                            testAnswers.AnsweredCodeSnippet = answer.Code;
                            testAnswers.TestConduct = testConduct;
                            await _dbContext.TestAnswers.AddAsync(testAnswers);
                            await _dbContext.SaveChangesAsync();
                        }
                    }
                }

                //Remove the answer from the AttendeeAnswer once Transformation is commenced 
                _dbContext.AttendeeAnswers.Remove(attendeeAnswer);
                await _dbContext.SaveChangesAsync();
            }
        }

        #endregion
    }
}