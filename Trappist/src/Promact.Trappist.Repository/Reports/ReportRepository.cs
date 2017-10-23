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
using Promact.Trappist.DomainModel.ApplicationClasses.Test;

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

        #region IReportRepository Methods
        #region Public Methods
        public async Task<TestAC> GetTestNameAsync(int id)
        {
            var result = await _dbContext.Test.AsNoTracking().Where(x => x.Id == id).Select(selectOnly => new { selectOnly.TestName, selectOnly.Link, }).ToListAsync();
            var testACObject = new TestAC()
            {
                TestName = result.First().TestName,
                Link = result.First().Link
            };
            return testACObject;
        }

        public async Task<IEnumerable<TestAttendees>> GetAllTestAttendeesAsync(int id)
        {
            return await _dbContext.TestAttendees.AsNoTracking().Where(t => t.TestId == id).Include(x => x.Report).ToListAsync();
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
            var listOfQuestionsInTest = await _dbContext.TestQuestion.Include(x => x.Question).Where(x => x.TestId == testId).ToListAsync();
            var testQuestionList = new List<TestQuestion>();
            foreach (var question in listOfQuestionsInTest)
            {
                if (question.Question.QuestionType == QuestionType.Programming)
                    testQuestionList.Add(question);
                else
                {
                    var testQuestionObject = await _dbContext.TestQuestion.Include(x => x.Question).Include(x => x.Question.SingleMultipleAnswerQuestion).ThenInclude(x => x.SingleMultipleAnswerQuestionOption).Where(x => x.QuestionId == question.QuestionId && x.TestId == testId).FirstOrDefaultAsync();
                    testQuestionList.Add(testQuestionObject);
                }
            }
            return testQuestionList;
        }

        public async Task<List<TestAnswers>> GetTestAttendeeAnswers(int testAttendeeId)
        {
            var testAttendeeFullAnswerList = await _dbContext.TestAnswers.Where(x => x.TestConduct.TestAttendeeId == testAttendeeId).ToListAsync();
            return testAttendeeFullAnswerList;
        }

        public async Task<double> CalculatePercentileAsync(int testAttendeeId, int testId)
        {
            int sameMarks = 0;
            int count = 0;
            double studentPercentile;

            var attendee = await _dbContext.Report.Where(x => x.TestAttendeeId == testAttendeeId).FirstOrDefaultAsync();
            var marksList = await _dbContext.Report.Where(x => x.TestAttendee.TestId == testId).OrderBy(x => x.TotalMarksScored).ToListAsync();
            double noOfScores = marksList.Count();

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
            var countOptions = 0; var totalQuestionAttempted = 0; var sameMarks = 0;
            var count = 0;

            //all testattendees Of a test
            var allTestAttendeeList = await _dbContext.TestAttendees.Where(x => x.TestId == testId).Include(x => x.Report).Select(x => x.Id).ToListAsync();
            //all testAttendees marks list
            var marksList = await _dbContext.Report.Where(x => x.TestAttendee.TestId == testId).OrderBy(x => x.TotalMarksScored).Select(s => new { s.TestAttendeeId, s.TotalMarksScored }).ToListAsync();
            //no test questions of a test
            var totalNoOfTestQuestions = await _dbContext.TestQuestion.CountAsync(x => x.TestId == testId);
            //all questions attempted by all attendees
            var questionsAttemptedByAllAttendeeList = _dbContext.TestConduct.Where(x => x.TestAttendees.TestId == testId && x.QuestionStatus == QuestionStatus.answered || x.QuestionStatus == QuestionStatus.review)
                                                           .Select(selectOnly => new { selectOnly.Id, selectOnly.TestAttendeeId, selectOnly.QuestionId, selectOnly.Question.QuestionType, selectOnly.Question.DifficultyLevel });
            //all answers attempted by all attendees
            var answeredByAllAttendeeList = await _dbContext.TestAnswers.Where(x => x.TestConduct.TestAttendees.TestId == testId).Select(s => new { s.TestConductId, s.AnsweredOption }).ToListAsync();

            //all testcode solutions of a test
            var testCodeSolutionsList =  _dbContext.TestCodeSolution.Where(x => x.TestAttendee.TestId == testId).Select(selectOnly => new { selectOnly.QuestionId, selectOnly.TestAttendeeId, selectOnly.Score });

            foreach (var testAttendee in allTestAttendeeList)
            {
                double studentPercentile = 0.0;
                var attemptedQuestions = questionsAttemptedByAllAttendeeList.Where(x => x.TestAttendeeId == testAttendee).ToList();
                attemptedQuestions.ForEach(x =>
                {
                    var checkAttempts = 0;
                    var difficultyLevel = x.DifficultyLevel;
                    if (x.QuestionType != QuestionType.Programming)
                    {
                        var givenOptionsByAttendee = answeredByAllAttendeeList.Where(y => y.TestConductId == x.Id).Select(y => y.AnsweredOption).FirstOrDefault();
                        if (givenOptionsByAttendee != null)
                        {
                            totalQuestionAttempted += 1;
                            checkAttempts = 1;
                        }
                    }
                    else
                    {
                        var givenSolutionByAttendee = testCodeSolutionsList.Where(y => y.QuestionId == x.QuestionId && y.TestAttendeeId == testAttendee).ToList();
                        if (givenSolutionByAttendee.Count() > 0)
                        {
                            totalQuestionAttempted += 1;
                            checkAttempts = 1;
                        }
                    }
                    if (checkAttempts == 1)
                    {
                        if (difficultyLevel == DifficultyLevel.Easy)
                            easyQuestionAttempted += 1;
                        else if (difficultyLevel == DifficultyLevel.Medium)
                            mediumQuestionAttempted += 1;
                        else
                            hardQuestionAttempted += 1;
                    }
                });
                //calculate percentile
                double noOfScores = marksList.Count();
                var attendee = marksList.FirstOrDefault(z => z.TestAttendeeId == testAttendee);
                foreach (var marks in marksList)
                {
                    if (attendee != null && attendee.TotalMarksScored == marks.TotalMarksScored)
                        sameMarks = sameMarks + 1;
                    else if (attendee != null && marks.TotalMarksScored < attendee.TotalMarksScored)
                        count = count + 1;
                }
                var rank = count + (0.5 * sameMarks);
                double percentile = rank / noOfScores;
                studentPercentile = percentile * 100;

                var reportQuestions = new ReportQuestionsCountAC()
                {
                    TestAttendeeId = testAttendee,
                    EasyQuestionAttempted = easyQuestionAttempted,
                    MediumQuestionAttempted = mediumQuestionAttempted,
                    HardQuestionAttempted = hardQuestionAttempted,
                    NoOfQuestionAttempted = totalQuestionAttempted,
                    Percentile = Math.Round(studentPercentile, 2),
                    totalTestQuestions = totalNoOfTestQuestions
                };
                allAttendeeMarksDetailsList.Add(reportQuestions);

                easyQuestionAttempted = mediumQuestionAttempted = hardQuestionAttempted = countOptions = totalQuestionAttempted = 0;
                studentPercentile = 0.0;
            };
            return allAttendeeMarksDetailsList;
        }

        public async Task<List<CodeSnippetTestCasesCalculationAC>> GetCodeSnippetDetailsAsync(int attendeeId, int questionId)
        {
            if (!await _dbContext.TestCodeSolution.Where(x => x.TestAttendeeId == attendeeId).AnyAsync(x => x.QuestionId == questionId))
                return null;
            var testCodeSolutionObject = await _dbContext.TestCodeSolution.Include(x => x.TestCaseResultCollection).OrderByDescending(x => x.CreatedDateTime).FirstOrDefaultAsync(x => x.TestAttendeeId == attendeeId && x.QuestionId == questionId);
            var codeSnippetTestCaseList = new List<CodeSnippetTestCasesCalculationAC>();
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
            var maximumScoreObtainedByAttendeeInCodeSnippetQuestion = await _dbContext.TestCodeSolution.OrderByDescending(x => x.CreatedDateTime).Where(x => x.TestAttendeeId == attendeeId && x.QuestionId == questionId).Select(x => x.Score).FirstOrDefaultAsync();
            var testObjectCorrectMarks = await _dbContext.TestAttendees.Where(x => x.Id == attendeeId).Include(x => x.Test).Select(x => x.Test.CorrectMarks).FirstOrDefaultAsync();
            var totalMarksObtainedInCodeSnippetQuestion = (decimal)maximumScoreObtainedByAttendeeInCodeSnippetQuestion * testObjectCorrectMarks;
            totalMarksObtainedInCodeSnippetQuestion = Math.Round(totalMarksObtainedInCodeSnippetQuestion, 2);
            return totalMarksObtainedInCodeSnippetQuestion;
        }

        public async Task<TestCodeSolutionDetailsAC> GetTestCodeSolutionDetailsAsync(int attendeeId, int questionId)
        {
            if (!await _dbContext.TestCodeSolution.Where(x => x.TestAttendeeId == attendeeId).AnyAsync(x => x.QuestionId == questionId))
                return null;
            var testCodeSolutionObject = await _dbContext.TestCodeSolution.Include(x => x.TestCaseResultCollection).OrderByDescending(x => x.CreatedDateTime).FirstOrDefaultAsync(x => x.TestAttendeeId == attendeeId && x.QuestionId == questionId);
            var totalNumberOfSuccessfulAttempts = await _dbContext.TestCodeSolution.CountAsync(x => x.Score == 1 && x.TestAttendeeId == attendeeId && x.QuestionId == questionId);
            var totalNumberOfAttemptsMadeByAttendee = await _dbContext.TestCodeSolution.CountAsync(x => x.TestAttendeeId == attendeeId && x.QuestionId == questionId);

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
                {
                    attendee.Report.IsTestPausedUnWillingly = false;
                    if (attendee.Report.TestStatus == TestStatus.AllCandidates)
                        attendee.Report.TestStatus = TestStatus.CompletedTest;
                }
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
                        if ((answer.AnsweredOption != null && count == 1 || conduct.IsAnswered))
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
            foreach (var id in attendeeIdList)
            {
                await _testConductRepository.SetAttendeeTestStatusAsync(id, TestStatus.UnfinishedTest);
            }

            return await _dbContext.TestAttendees.Where(x => attendeeIdList.Any(id => id == x.Id)).Include(x => x.Report).ToListAsync();
        }

        public async Task<List<int>> GetAttendeeIdListAsync(int testId)
        {
            var testAttendeeIdList = await _dbContext.TestAttendees.Where(x => x.TestId == testId && x.Report != null).Select(x => x.Id).ToListAsync();
            return testAttendeeIdList;
        }
        #endregion
        #endregion
    }
}