using Microsoft.EntityFrameworkCore;
using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.Models.TestConduct;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Promact.Trappist.DomainModel.Enum;
using Promact.Trappist.DomainModel.Models.Test;
using Promact.Trappist.DomainModel.ApplicationClasses.Reports;
using Promact.Trappist.DomainModel.Models.Question;
using System;
using Promact.Trappist.DomainModel.Models.Report;
using Promact.Trappist.Repository.TestConduct;

namespace Promact.Trappist.Repository.Reports
{
    public class ReportRepository : IReportRepository
    {
        #region Private Members
        private readonly TrappistDbContext _dbContext;
        private readonly ITestConductRepository _testConductRepository; 
        #endregion

        #region Constructor
        public ReportRepository(TrappistDbContext dbContext, ITestConductRepository testConductRepository)
        {
            _dbContext = dbContext;
            _testConductRepository = testConductRepository;
        }
        #endregion

        #region Public Method
        public async Task<Test> GetTestNameAsync(int id)
        {
            return await _dbContext.Test.FindAsync(id);

        }

        public async Task<IEnumerable<TestAttendees>> GetAllTestAttendeesAsync(int id)
        {
            return await _dbContext.TestAttendees.Where(t => t.TestId == id).Include(x => x.Report).ToListAsync();
        }

        public async Task SetStarredCandidateAsync(int id)
        {
            var studentToBeStarred = await _dbContext.TestAttendees.FindAsync(id);
            studentToBeStarred.StarredCandidate = !studentToBeStarred.StarredCandidate;
            await _dbContext.SaveChangesAsync();
        }

        public async Task SetAllCandidateStarredAsync(bool status, int selectedTestStatus, string searchString)
        {
            List<TestAttendees> attendeeList;

            if (searchString == null)
                searchString = "";
            else
                searchString = searchString.ToLowerInvariant();

            if ((TestStatus)selectedTestStatus == TestStatus.AllCandidates)
                attendeeList = await _dbContext.TestAttendees.Where(x => x.FirstName.ToLower().Contains(searchString) || x.LastName.ToLower().Contains(searchString) || x.Email.Contains(searchString)).ToListAsync();

            else
                attendeeList = await _dbContext.TestAttendees.Include(x => x.Report).Where(x => x.Report.TestStatus == (TestStatus)selectedTestStatus
                && (x.FirstName.ToLower().Contains(searchString) || x.LastName.ToLower().Contains(searchString) || x.Email.Contains(searchString))).ToListAsync();

            attendeeList.ForEach(x => x.StarredCandidate = status);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> IsCandidateExistAsync(int attendeeId)
        {
            return await _dbContext.TestAttendees.AnyAsync(s => s.Id == attendeeId);
        }

        public async Task<TestAttendees> GetTestAttendeeDetailsByIdAsync(int testAttendeeId)
        {
            var testAttendee = await _dbContext.TestAttendees.Include(x => x.Test).Include(x => x.TestConduct).Include(x => x.TestLogs).Include(x => x.Report).FirstOrDefaultAsync(x => x.Id == testAttendeeId);
            return testAttendee;
        }

        public async Task<List<TestQuestion>> GetTestQuestions(int testId)
        {
            return await _dbContext.TestQuestion.Include(x => x.Question).Include(x => x.Question.SingleMultipleAnswerQuestion).ThenInclude(x => x.SingleMultipleAnswerQuestionOption).Where(x => x.TestId == testId).ToListAsync();
        }

        public async Task<List<TestAnswers>> GetTestAttendeeAnswers(int testAttendeeId)
        {
            var testAttendeeQuestionList = await _dbContext.TestConduct.Where(x => x.TestAttendeeId == testAttendeeId).ToListAsync();
            var testAttendeeFullAnswerList = new List<TestAnswers>();

            foreach (DomainModel.Models.TestConduct.TestConduct testAttendeeQuestion in testAttendeeQuestionList)
            {
                var testAttendeeAnswerList = await _dbContext.TestAnswers.Where(x => x.TestConductId == testAttendeeQuestion.Id).ToListAsync();
                testAttendeeFullAnswerList.AddRange(testAttendeeAnswerList);
            }
            return testAttendeeFullAnswerList;
        }

        public async Task<double> CalculatePercentileAsync(int testAttendeeId)
        {
            var testAttendee = await _dbContext.TestAttendees.Include(x => x.Test).FirstOrDefaultAsync(x => x.Id == testAttendeeId);
            int sameMarks = 0;
            int count = 0;
            var attendeeList = await _dbContext.TestAttendees.Include(x => x.Report).Where(x => x.TestId == testAttendee.TestId).ToListAsync();
            double noOfScores = attendeeList.Count();
            var attendee = await _dbContext.Report.Where(x => x.TestAttendeeId == testAttendeeId).FirstOrDefaultAsync();
            double studentPercentile;

            var marksList = await _dbContext.Report.Where(x => x.TestAttendee.TestId == testAttendee.TestId).OrderBy(x => x.TotalMarksScored).ToListAsync();
            foreach (var marks in marksList)
            {
                if (attendee.TotalMarksScored == marks.TotalMarksScored)
                    sameMarks = sameMarks + 1;
                else if (marks.TotalMarksScored < attendee.TotalMarksScored)
                    count = count + 1;
            }

            var rank = count + (0.5 * sameMarks);
            double percentile = rank / noOfScores;
            studentPercentile = percentile * 100;
            return studentPercentile;
        }

        public async Task<List<ReportQuestionsCountAC>> GetAllAttendeeMarksDetailsAsync(int testId)
        {
            var testAttendeeAnswerList = new List<TestAnswers>();
            var testQuestionList = new List<TestQuestion>();
            var allAttendeeMarksDetailsList = new List<ReportQuestionsCountAC>();
            var easyQuestionAttempted = 0; var hardQuestionAttempted = 0; var mediumQuestionAttempted = 0;
            var correctAttemptedQuestion = 0; var totalCorrectOptions = 0; var countOptions = 0; var totalQuestionAttempted = 0;

            //all testattendees Of a test
            var allTestAttendeeList = await _dbContext.TestAttendees.Where(x => x.TestId == testId && x.Report != null).Select(x=>x.Id).ToListAsync();
            //all test questions of a test
            testQuestionList = await GetTestQuestions(testId);
            var totalNoOfTestQuestions = testQuestionList.Count();
            //all questions and answers attempted by all attendees
            var questionsAttemptedByAllAttendeeList = await _dbContext.TestConduct.Where(x => x.QuestionStatus == QuestionStatus.answered || x.QuestionStatus == QuestionStatus.review)
                                                                                  .Include(x => x.TestAnswers).ToListAsync();
            //all testcode solutions of a test
            var testCodeSolutionsList = await _dbContext.TestCodeSolution.Select(selectOnly => new { selectOnly.QuestionId, selectOnly.TestAttendeeId ,selectOnly.Score}).ToListAsync();

            foreach (var testAttendee in allTestAttendeeList)
            {

                var attemptedQuestions = questionsAttemptedByAllAttendeeList.Where(x => x.TestAttendeeId == testAttendee).ToList();
                attemptedQuestions.ForEach(x =>
                {
                    if (!(x.Question.QuestionType == QuestionType.Programming))
                    {
                        var difficultyLevel = x.Question.DifficultyLevel;
                        var question = testQuestionList.FirstOrDefault(y => y.QuestionId == x.QuestionId);
                        if (x.Question.QuestionType==QuestionType.Single)
                        {
                            var correctOption = question.Question.SingleMultipleAnswerQuestion.SingleMultipleAnswerQuestionOption.Where(z => z.SingleMultipleAnswerQuestionID == x.Question.Id && z.IsAnswer).Select(z => z.Id).ToList();
                            var givenOptionsByAttendee = x.TestAnswers.Where(y => y.TestConductId == x.Id).Select(y => y.AnsweredOption).ToList();
                            if (givenOptionsByAttendee.First() != null)
                            {
                                totalQuestionAttempted += 1;
                                if (givenOptionsByAttendee.First().Value == correctOption.First())
                                    correctAttemptedQuestion += 1;
                                if (difficultyLevel == DifficultyLevel.Easy)
                                    easyQuestionAttempted += 1;
                                else if(difficultyLevel == DifficultyLevel.Medium)
                                        mediumQuestionAttempted += 1;
                                    else
                                        hardQuestionAttempted += 1;
                            }
                        }
                        else
                        {                        
                            var Options = question.Question.SingleMultipleAnswerQuestion.SingleMultipleAnswerQuestionOption.Where(y => y.SingleMultipleAnswerQuestionID == x.Question.Id).ToList();
                            totalCorrectOptions = Options.Where(y => y.IsAnswer).Count();
                            var givenOptionsByAttendee = x.TestAnswers.Where(y => y.TestConductId == x.Id).Select(y => y.AnsweredOption).ToList();
                            if (givenOptionsByAttendee.First() != null)
                            {
                                totalQuestionAttempted += 1;
                                givenOptionsByAttendee.ForEach(z =>
                                {
                                    var option = z.Value;
                                    if (Options.Find(c => c.Id == option).IsAnswer)
                                        countOptions += 1;
                                    if (!Options.Find(c => c.Id == option).IsAnswer)
                                        countOptions -= 1;
                                });
                                if (totalCorrectOptions == countOptions)
                                    correctAttemptedQuestion += 1;
                                if (difficultyLevel == DifficultyLevel.Easy)
                                    easyQuestionAttempted += 1;
                                else if (difficultyLevel == DifficultyLevel.Medium)
                                        mediumQuestionAttempted += 1;
                                    else
                                        hardQuestionAttempted += 1;
                            }
                        }                        
                    }
                    else 
                    {
                        var difficultyLevel = x.Question.DifficultyLevel;
                        var givenSolutionByAttendee = testCodeSolutionsList.Where(y => y.QuestionId == x.QuestionId && y.TestAttendeeId == testAttendee).ToList();
                        if(givenSolutionByAttendee.Count() >0)
                        {
                            totalQuestionAttempted += 1;
                            if (difficultyLevel == DifficultyLevel.Easy)
                                easyQuestionAttempted += 1;
                            else
                            {
                                if (difficultyLevel == DifficultyLevel.Medium)
                                    mediumQuestionAttempted += 1;
                                else
                                    hardQuestionAttempted += 1;
                            }
                        }
                        if (givenSolutionByAttendee.Any(y => y.Score == 1 && y.QuestionId == x.QuestionId && y.TestAttendeeId == testAttendee))
                            correctAttemptedQuestion += 1;
                       
                    }
                });
                var percentile = await CalculatePercentileAsync(testAttendee);
                var reportQuestions = new ReportQuestionsCountAC()
                {
                    TestAttendeeId = testAttendee,
                    EasyQuestionAttempted = easyQuestionAttempted,
                    MediumQuestionAttempted = mediumQuestionAttempted,
                    HardQuestionAttempted = hardQuestionAttempted,
                    CorrectQuestionsAttempted = correctAttemptedQuestion,
                    NoOfQuestionAttempted = totalQuestionAttempted,
                    Percentile = System.Math.Round(percentile, 2),
                    totalTestQuestions = totalNoOfTestQuestions
                };
                allAttendeeMarksDetailsList.Add(reportQuestions);
                
                easyQuestionAttempted = mediumQuestionAttempted = hardQuestionAttempted = correctAttemptedQuestion = countOptions = totalQuestionAttempted = 0;

            };
            return allAttendeeMarksDetailsList;
        }

        public async Task<List<CodeSnippetTestCasesCalculationAC>> GetCodeSnippetDetailsAsync(int attendeeId, int questionId)
        {
            if (!await _dbContext.TestCodeSolution.Where(x => x.TestAttendeeId == attendeeId).AnyAsync(x => x.QuestionId == questionId))
            {
                return null;
            }
            var codeSnippetTestCaseList = new List<CodeSnippetTestCasesCalculationAC>();
            var maximumScore = await _dbContext.TestCodeSolution.Where(x => x.TestAttendeeId == attendeeId && x.QuestionId == questionId).MaxAsync(x => x.Score);
            var testCodeSolutionObject = await _dbContext.TestCodeSolution.Include(x => x.TestCaseResultCollection).FirstOrDefaultAsync(x => x.Score == maximumScore && x.TestAttendeeId == attendeeId && x.QuestionId == questionId);
            var testCaseObjectList = testCodeSolutionObject.TestCaseResultCollection;

            bool testCasePassed;

            var codeSnippetQuestionTestCasesList = await _dbContext.CodeSnippetQuestionTestCases.Where(x => x.CodeSnippetQuestionId == questionId).ToListAsync();
            var i = 0;
            foreach (var testCase in testCaseObjectList)
            {
                var codeSnippetTestCase = codeSnippetQuestionTestCasesList.Where(x => x.Id == testCase.CodeSnippetQuestionTestCasesId).Single();
                testCasePassed = testCase.Output == codeSnippetTestCase.TestCaseOutput;
                var testCaseDetailsList = new CodeSnippetTestCasesCalculationAC()
                {
                    TestCaseName = codeSnippetTestCase.TestCaseTitle,
                    TestCaseInput = codeSnippetTestCase.TestCaseInput,
                    TestCaseMarks = codeSnippetTestCase.TestCaseMarks,
                    TestCaseType = codeSnippetTestCase.TestCaseType,
                    ExpectedOutput = codeSnippetTestCase.TestCaseOutput,
                    Processing = testCase.Processing,
                    Memory = testCase.Memory,
                    ActualOutput = testCase.Output,
                    IsTestCasePassing = testCasePassed
                };
                i++;
                codeSnippetTestCaseList.Add(testCaseDetailsList);
            }
            return codeSnippetTestCaseList;
        }

        public async Task<decimal> GetTotalMarksOfCodeSnippetQuestionAsync(int attendeeId, int questionId)
        {
            if (!await _dbContext.TestCodeSolution.Where(x => x.TestAttendeeId == attendeeId).AnyAsync(x => x.QuestionId == questionId))
            {
                return -1;
            }
            var maximumScoreObtainedByAttendeeInCodeSnippetQuestion = await _dbContext.TestCodeSolution.Where(x => x.TestAttendeeId == attendeeId && x.QuestionId == questionId).MaxAsync(x => x.Score);
            var testObject = await _dbContext.TestAttendees.Include(x => x.Test).FirstOrDefaultAsync(x => x.Id == attendeeId);
            var totalMarksObtainedInCodeSnippetQuestion = (decimal)maximumScoreObtainedByAttendeeInCodeSnippetQuestion * testObject.Test.CorrectMarks;
            totalMarksObtainedInCodeSnippetQuestion = Math.Round(totalMarksObtainedInCodeSnippetQuestion, 2);
            return totalMarksObtainedInCodeSnippetQuestion;
        }

        public async Task<TestCodeSolutionDetailsAC> GetTestCodeSolutionDetailsAsync(int attendeeId, int questionId)
        {
            if (!await _dbContext.TestCodeSolution.Where(x => x.TestAttendeeId == attendeeId).AnyAsync(x => x.QuestionId == questionId))
            {
                return null;
            }
            var maximumScore = await _dbContext.TestCodeSolution.Where(x => x.TestAttendeeId == attendeeId && x.QuestionId == questionId).MaxAsync(x => x.Score);
            var testCodeSolutionObject = await _dbContext.TestCodeSolution.FirstOrDefaultAsync(x => x.Score == maximumScore && x.TestAttendeeId == attendeeId && x.QuestionId == questionId);
            var testCodeSolutionListOfMaximumScore = await _dbContext.TestCodeSolution.Where(x => x.Score == 1 && x.TestAttendeeId == attendeeId && x.QuestionId == questionId).ToListAsync();
            var totalNumberOfSuccessfulAttempts = testCodeSolutionListOfMaximumScore.Count();
            var testCodeSolutionList = await _dbContext.TestCodeSolution.Where(x => x.TestAttendeeId == attendeeId && x.QuestionId == questionId).ToListAsync();
            var totalNumberOfAttemptsMadeByAttendee = testCodeSolutionList.Count();

            var testCodeSolutionDetailsObject = new TestCodeSolutionDetailsAC()
            {
                Language = testCodeSolutionObject.Language,
                NumberOfSuccessfulAttempts = totalNumberOfSuccessfulAttempts,
                TotalNumberOfAttempts = totalNumberOfAttemptsMadeByAttendee,
                CodeSolution = testCodeSolutionObject.Solution
            };

            return testCodeSolutionDetailsObject;
        }

        public async Task<TestAttendees> SetTestStatusAsync(TestAttendees attendee, bool isTestEnd)
        {
            attendee.Report = await _dbContext.Report.FirstOrDefaultAsync(x => x.TestAttendeeId == attendee.Id);
            if (attendee.Report != null)
            {
                if (isTestEnd)
                    attendee.Report.IsTestPausedUnWillingly = false;
                else
                {
                    attendee.Report.IsTestPausedUnWillingly = false;
                    attendee.Report.IsAllowResume = true;
                    attendee.Report.TestStatus = TestStatus.AllCandidates;
                }
                _dbContext.Report.Update(attendee.Report);
                await _dbContext.SaveChangesAsync();
                return attendee;
            }
            else
                return null;
        }

        public async Task SetWindowCloseAsync(int attendeeId, bool isTestResume)
        {
            var reportObject = await _dbContext.Report.FirstOrDefaultAsync(x => x.TestAttendeeId == attendeeId);
            if (reportObject != null)
            {
                if (isTestResume)
                    reportObject.IsAllowResume = false;
                else
                    reportObject.IsTestPausedUnWillingly = true;
                _dbContext.Report.Update(reportObject);
                await _dbContext.SaveChangesAsync();
            }
        }
        public async Task<Report> GetWindowCloseAsync(int attendeeId)
        {
            var reportObject = await _dbContext.Report.FirstOrDefaultAsync(x => x.TestAttendeeId == attendeeId);
            return reportObject;
        }

        public async Task<int> GetAttemptedQuestionsByAttendeeAsync(int attendeeId)
        {
            var numberOfAnsweredButReviewedQuestions = 0;
            var count = 0;
            var listOfQuestionsAttemptedByTestAttendee = await _dbContext.TestConduct.Include(x => x.TestAnswers).Where(x => x.TestAttendeeId == attendeeId && x.QuestionStatus == QuestionStatus.answered).ToListAsync();
            var listOfQuestionsAttendedByTestAttendee = await _dbContext.TestConduct.Include(x => x.TestAnswers).Where(x => x.TestAttendeeId == attendeeId && x.QuestionStatus == QuestionStatus.review).ToListAsync();

            foreach (var conduct in listOfQuestionsAttendedByTestAttendee)
            {
                count = 0;
                if (conduct.QuestionStatus == QuestionStatus.unanswered || conduct.QuestionStatus == QuestionStatus.selected)
                {
                    continue;
                }

                if (conduct.QuestionStatus == QuestionStatus.review)
                {
                    foreach (var answer in conduct.TestAnswers)
                    {
                        if (conduct.Id == answer.TestConductId)
                            count = count + 1;
                        if (answer.AnsweredOption != null && count == 1)
                        {
                            numberOfAnsweredButReviewedQuestions = numberOfAnsweredButReviewedQuestions + 1;
                        }
                    }
                }
            }

            var numberOfQuestionsAttempted = listOfQuestionsAttemptedByTestAttendee.Count() + numberOfAnsweredButReviewedQuestions;
            return numberOfQuestionsAttempted;
        }

        public async Task<List<TestAttendees>> GenerateReportForUnfinishedTestAsync(List<int> attendeeIdList)
        {
            foreach(var id in attendeeIdList)
            {
                await _testConductRepository.SetAttendeeTestStatusAsync(id, TestStatus.UnfinishedTest);
            }

            return await _dbContext.TestAttendees.Where(x => attendeeIdList.Any(id => id == x.Id)).Include(x => x.Report).ToListAsync();
        }
        #endregion
    }
}