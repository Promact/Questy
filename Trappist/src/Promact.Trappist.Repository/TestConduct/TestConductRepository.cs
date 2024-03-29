﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Promact.Trappist.DomainModel.ApplicationClasses.CodeSnippet;
using Promact.Trappist.DomainModel.ApplicationClasses.TestConduct;
using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.Enum;
using Promact.Trappist.DomainModel.Models.Question;
using Promact.Trappist.DomainModel.Models.Report;
using Promact.Trappist.DomainModel.Models.TestConduct;
using Promact.Trappist.DomainModel.Models.TestLogs;
using Promact.Trappist.Utility.Constants;
using Promact.Trappist.Utility.HttpUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Promact.Trappist.Repository.Test;

namespace Promact.Trappist.Repository.TestConduct
{
    public class TestConductRepository : ITestConductRepository
    {
        #region Private Variables
        #region Dependencies
        private readonly TrappistDbContext _dbContext;
        private readonly ITestsRepository _testRepository;
        private readonly IConfiguration _configuration;
        private readonly IStringConstants _stringConstants;
        private readonly IHttpService _httpService;
        #endregion
        #endregion

        #region Constructor
        public TestConductRepository(TrappistDbContext dbContext
            , ITestsRepository testRepository
            , IConfiguration configuration, IStringConstants stringConstants
            , IHttpService httpService)
        {
            _dbContext = dbContext;
            _testRepository = testRepository;
            _configuration = configuration;
            _stringConstants = stringConstants;
            _httpService = httpService;
        }
        #endregion


        #region Public Method
        public async Task RegisterTestAttendeesAsync(TestAttendees testAttendee)
        {
            await _dbContext.TestAttendees.AddAsync(testAttendee);
            await _dbContext.SaveChangesAsync();
            var testLogsObject = new TestLogs();
            testLogsObject.TestAttendeeId = testAttendee.Id;
            testLogsObject.VisitTestLink = DateTime.UtcNow;
            testLogsObject.FillRegistrationForm = DateTime.UtcNow;
            await _dbContext.TestLogs.AddAsync(testLogsObject);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> IsTestAttendeeExistAsync(TestAttendees testAttendee, string magicString)
        {
            var testObject = (await _dbContext.Test.FirstOrDefaultAsync(x => (x.Link == magicString)));
            if (testObject != null)
            {
                testAttendee.TestId = testObject.Id;
                var isTestAttendeeExist = await _dbContext.TestAttendees.AnyAsync(x => x.Email == testAttendee.Email && x.TestId == testAttendee.TestId && x.RollNumber == testAttendee.RollNumber);
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

        public async Task<bool> IsTestLinkExistForTestConductionAsync(string magicString, string userIp)
        {
            var testObject = await _dbContext.Test.Where(x => x.Link == magicString).Include(x => x.TestIpAddress).FirstOrDefaultAsync();
            var currentDate = DateTime.UtcNow;
            // if Test is not paused and current date is not greater than EndDate and machine IP address is in the list of test ip addresses of  then it returns true and test link exist otherwise link does not exist
            if (testObject != null && testObject.TestIpAddress.Count != 0)
                return DateTime.Compare(currentDate, testObject.StartDate) >= 0 && DateTime.Compare(currentDate, testObject.EndDate) < 0 && !testObject.IsPaused && testObject.TestIpAddress.Any(x => x.IpAddress.Equals(userIp)) && testObject.IsLaunched;
            return testObject != null && DateTime.Compare(currentDate, testObject.StartDate) >= 0 && DateTime.Compare(currentDate, testObject.EndDate) < 0 && !testObject.IsPaused && testObject.IsLaunched;
        }

        public async Task AddAnswerAsync(int attendeeId, TestAnswerAC answer, double seconds)
        {
            var attendeeAnswer = await _dbContext.AttendeeAnswers.Where(x => x.Id == attendeeId).FirstOrDefaultAsync();

            if (attendeeAnswer != null)
            {
                List<TestAnswerAC> deserializedAnswer = null;

                if (attendeeAnswer.Answers != null)
                {
                    deserializedAnswer = (JsonConvert.DeserializeObject<TestAnswerAC[]>(attendeeAnswer.Answers) ?? Array.Empty<TestAnswerAC>()).ToList();
                }

                //Remove answer if already exist
                var answerToUpdate = deserializedAnswer?.SingleOrDefault(x => x.QuestionId == answer.QuestionId);
                if (answerToUpdate != null)
                {
                    deserializedAnswer.Remove(answerToUpdate);
                }

                //Add answer
                if (deserializedAnswer != null)
                {
                    deserializedAnswer.Add(answer);
                    var serializedAnswer = JsonConvert.SerializeObject(deserializedAnswer);
                    attendeeAnswer.Answers = serializedAnswer;
                }

                _dbContext.AttendeeAnswers.Update(attendeeAnswer);

            }
            else
            {
                attendeeAnswer = new AttendeeAnswers();
                attendeeAnswer.Id = attendeeId;
                if (seconds != 0.0)
                    attendeeAnswer.TimeElapsed = (seconds / 60d);

                if (answer != null)
                {
                    var testAnswerArray = new List<TestAnswerAC>();
                    testAnswerArray.Add(answer);
                    attendeeAnswer.Answers = JsonConvert.SerializeObject(testAnswerArray);
                }
                await _dbContext.AddAsync(attendeeAnswer);
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ICollection<TestAnswerAC>> GetAnswerAsync(int attendeeId)
        {
            var attendee = await _dbContext.AttendeeAnswers.AsNoTracking().SingleOrDefaultAsync(x => x.Id == attendeeId);

            if (attendee == null || attendee.Answers == null)
                return null;

            var deserializedAttendeeAnswers = JsonConvert.DeserializeObject<ICollection<TestAnswerAC>>(attendee.Answers);
            return deserializedAttendeeAnswers;
        }

        public async Task<TestAttendees> GetTestAttendeeByIdWithoutReportAsync(int attendeeId)
        {
            var testAttendee = await _dbContext.TestAttendees.AsNoTracking().SingleAsync(x => x.Id == attendeeId);
            return testAttendee;
        }

        public async Task<TestAttendees> GetTestAttendeeByIdAsync(int attendeeId)
        {
            var testAttendee = await _dbContext.TestAttendees.AsNoTracking().Include(x => x.Report).SingleAsync(x => x.Id == attendeeId);
            return testAttendee;
        }

        public async Task<bool> IsTestAttendeeExistByIdAsync(int attendeeId)
        {
            return await _dbContext.TestAttendees.AnyAsync(x => x.Id == attendeeId);
        }

        public async Task SetElapsedTimeAsync(int attendeeId, long seconds, bool isDisconnected)
        {
            var attendee = await _dbContext.AttendeeAnswers.FindAsync(attendeeId);
            if (attendee != null)
            {
                if (isDisconnected)
                    attendee.TimeElapsed += (seconds / 60d);
                else
                    attendee.TimeElapsed = (seconds / 60d);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                await AddAnswerAsync(attendeeId, null, seconds);
            }
        }

        public async Task<double> GetElapsedTimeAsync(int attendeeId)
        {
            var attendeeAnswer = await _dbContext.AttendeeAnswers.FindAsync(attendeeId);

            if (attendeeAnswer != null)
                return attendeeAnswer.TimeElapsed;
            return 0.0;
        }

        public async Task<TestAttendees> SetAttendeeTestStatusAsync(int attendeeId, TestStatus testStatus)
        {
            var testAttendee = await _dbContext.TestAttendees.Where(x => x.Id == attendeeId).Include(x => x.Report).FirstAsync();
            if (testAttendee is { Report: null })
            {
                testAttendee.Report = new Report();
                testAttendee.Report.TestAttendeeId = attendeeId;
                await _dbContext.Report.AddAsync(testAttendee.Report);
            }

            testAttendee.Report.TestStatus = testStatus;
            await _dbContext.SaveChangesAsync();
            if (testStatus != TestStatus.AllCandidates)
            {
                //Begin transformation
                await TransformAttendeeAnswer(attendeeId);
                await GetTotalMarks(attendeeId);
            }

            var testLogs = await _dbContext.TestLogs.FirstAsync(x => x.TestAttendeeId == attendeeId);
            testLogs.FinishTest = DateTime.UtcNow;
            _dbContext.TestLogs.Update(testLogs);

            await _dbContext.SaveChangesAsync();

            await GetTimeTakenByAttendeeAsync(attendeeId);
            return testAttendee;

        }

        public async Task<TestStatus> GetAttendeeTestStatusAsync(int attendeeId)
        {
            var report = await _dbContext.Report.AsNoTracking().OrderBy(x => x.TestAttendeeId).FirstOrDefaultAsync(x => x.TestAttendeeId == attendeeId);
            if (report == null)
                return TestStatus.AllCandidates;

            return report.TestStatus;
        }

        public async Task<bool> IsTestInValidDateWindow(string testLink, bool isPreview)
        {
            if (isPreview)
                return true;
            var currentDate = DateTime.UtcNow;
            var testQuery = _dbContext.Test.Where(x => x.Link == testLink).Select(x => new { x.StartDate, x.EndDate });
            var test = await testQuery.SingleAsync();

            return currentDate.CompareTo(test.StartDate) >= 0 && currentDate.CompareTo(test.EndDate) <= 0;
        }

        public async Task<CodeResponse> ExecuteCodeSnippetAsync(int attendeeId, bool runOnlyDefault, TestAnswerAC testAnswer)
        {
            var allTestCasePassed = true;
            var countPassedTest = 0;
            var errorEncounter = false;
            var errorMessage = "";
            var score = 0d;
            var code = testAnswer.Code;
            var codeResponse = new CodeResponse();
            var testCases = new List<CodeSnippetQuestionTestCases>();
            var testCaseResults = new List<TestCaseResult>();
            var testCaseChecks = await _dbContext.CodeSnippetQuestion.SingleAsync(x => x.Id == testAnswer.QuestionId);

            var completeTestCases = await _dbContext.CodeSnippetQuestionTestCases.Where(x => x.CodeSnippetQuestionId == testAnswer.QuestionId).ToListAsync();

            //Filter Test Cases
            testCases.AddRange(completeTestCases.Where(x => x.TestCaseType == TestCaseType.Default).ToList());
            if (testCaseChecks.RunBasicTestCase && !runOnlyDefault)
                testCases.AddRange(completeTestCases.Where(x => x.TestCaseType == TestCaseType.Basic).ToList());
            if (testCaseChecks.RunCornerTestCase && !runOnlyDefault)
                testCases.AddRange(completeTestCases.Where(x => x.TestCaseType == TestCaseType.Corner).ToList());
            if (testCaseChecks.RunNecessaryTestCase && !runOnlyDefault)
                testCases.AddRange(completeTestCases.Where(x => x.TestCaseType == TestCaseType.Necessary).ToList());
            //End of filter

            foreach (var testCase in testCases)
            {
                code.Input = testCase.TestCaseInput;

                var result = await ExecuteCodeAsync(code);

                if (result.ExitCode != 0)
                {
                    errorEncounter = true;
                    errorMessage = result.Output;
                }

                //Trim newline character 
                result.Output = result.Output.TrimEnd('\r', '\n');

                if (result.Output != testCase.TestCaseOutput)
                {
                    allTestCasePassed = false;
                }
                else
                {
                    countPassedTest++;
                    //Calculate score
                    score += testCase.TestCaseMarks;
                }

                var testCaseResult = new TestCaseResult()
                {
                    Memory = result.MemoryConsumed,
                    Output = result.Output,
                    CodeSnippetQuestionTestCasesId = testCase.Id,
                    Processing = result.RunTime
                };
                testCaseResults.Add(testCaseResult);
            }

            //Add score to the TestCodeSolution table
            //Final score is calculated using the following formula:
            //Total score of this question = Sum(1st test case marks + 2nd test case marks .....) / Sum of total marks of all test cases
            var totalTestCaseScore = testCases.Sum(x => x.TestCaseMarks);
            var codeSolution = new TestCodeSolution()
            {
                TestAttendeeId = attendeeId,
                QuestionId = testAnswer.QuestionId,
                Solution = testAnswer.Code.Source,
                Language = testAnswer.Code.Language,
                Score = score / totalTestCaseScore
            };
            await _dbContext.TestCodeSolution.AddAsync(codeSolution);
            await _dbContext.SaveChangesAsync();

            //Add result to TestCaseResult Table
            foreach (var testCaseResult in testCaseResults)
            {
                testCaseResult.TestCodeSolution = codeSolution;
            }
            await _dbContext.TestCaseResult.AddRangeAsync(testCaseResults);
            await _dbContext.SaveChangesAsync();

            codeResponse.ErrorOccurred = false;
            if (allTestCasePassed && !errorEncounter)
            {
                codeResponse.Message = runOnlyDefault ? _stringConstants.DefaultTestCasePassed : _stringConstants.AllTestCasePassed;
            }
            else if (!errorEncounter)
            {
                if (countPassedTest > 0)
                {
                    codeResponse.Message = runOnlyDefault ? _stringConstants.DefaultTestCaseFailed : _stringConstants.SomeTestCasePassed;
                }
                else
                {
                    codeResponse.Message = runOnlyDefault ? _stringConstants.DefaultTestCaseFailed : _stringConstants.NoTestCasePassed;
                }
            }
            else
            {
                codeResponse.ErrorOccurred = true;
                codeResponse.Error = errorMessage;
            }

            //Add answer to the database
            testAnswer.Code.CodeResponse = codeResponse;
            codeResponse.TotalTestCases = testCases.Count();
            codeResponse.TotalTestCasePassed = countPassedTest;
            await AddAnswerAsync(attendeeId, testAnswer, 0.0);

            return codeResponse;
        }

        public async Task<CodeResponse> ExecuteCustomInputAsync(int attendeeId, TestAnswerAC testAnswer)
        {
            var result = new Result();
            if (testAnswer.Code.Input != null)
                result = await ExecuteCodeAsync(testAnswer.Code);

            var codeResponse = new CodeResponse()
            {
                ErrorOccurred = result.ExitCode != 0,
                Error = result.Output,
                Message = null,
                Output = result.Output,
                TimeConsumed = result.RunTime,
                MemoryConsumed = result.MemoryConsumed,
                TotalTestCasePassed = 0,
                TotalTestCases = 0
            };

            testAnswer.Code.CodeResponse = codeResponse;
            await AddAnswerAsync(attendeeId, testAnswer, 0.0);

            return codeResponse;
        }

        public async Task<bool> AddTestLogsAsync(int attendeeId, bool isCloseWindow, bool isTestResume)
        {
            var testLogs = await _dbContext.TestLogs.FirstOrDefaultAsync(x => x.TestAttendeeId == attendeeId);
            if (testLogs != null)
            {
                if (isCloseWindow)
                    testLogs.CloseWindowWithoutFinishingTest = DateTime.UtcNow;
                else if (isTestResume)
                    testLogs.ResumeTest = DateTime.UtcNow;
                else
                    testLogs.AwayFromTestWindow = DateTime.UtcNow;
                _dbContext.TestLogs.Update(testLogs);
                await _dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<int> GetTestSummaryDetailsAsync(string testLink)
        {
            var testId = await _dbContext.Test.AsNoTracking().Where(x => x.Link == testLink).Select(x => x.Id).FirstOrDefaultAsync();
            var questionCount = 0;

            if (testId > 0)
            {
                questionCount = await _dbContext.TestQuestion.Where(x => x.TestId == testId).CountAsync();
            }

            return questionCount;
        }

        public async Task<List<TestLogs>> GetTestLogsAsync()
        {
            return await _dbContext.TestLogs.ToListAsync();
        }

        public async Task<TestAttendees> GetTestAttendeeByEmailIdAndRollNo(string email, string rollno, int testId)
        {
            return await _dbContext.TestAttendees.FirstOrDefaultAsync(x => x.Email == email && x.RollNumber == rollno && x.TestId == testId);
        }

        public async Task SetAttendeeBrowserToleranceValueAsync(int attendeeId, int attendeeBrowserToleranceCount)
        {
            var attendeeBrowserToleranceValue = await _dbContext.TestAttendees.FirstAsync(x => x.Id == attendeeId);
            attendeeBrowserToleranceValue.AttendeeBrowserToleranceCount = attendeeBrowserToleranceCount;
            _dbContext.TestAttendees.Update(attendeeBrowserToleranceValue);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<DateTime> GetExpectedTestEndTime(double testDuration, int testId)
        {
            var resumedAttendee = await _dbContext.TestLogs.Include(x => x.TestAttendee).Where(x => x.TestAttendee.TestId == testId).OrderByDescending(x => x.ResumeTest).FirstAsync();
            var startedAttendee = await _dbContext.TestLogs.Include(x => x.TestAttendee).Where(x => x.TestAttendee.TestId == testId).OrderByDescending(x => x.StartTest).FirstOrDefaultAsync();
            var timeSpan = TimeSpan.FromMinutes(testDuration);

            if (startedAttendee == null)
                return DateTime.MinValue;

            if (resumedAttendee.ResumeTest == null || resumedAttendee.ResumeTest.Value <= startedAttendee.StartTest)
            {
                if (startedAttendee.FinishTest != DateTime.MinValue && startedAttendee.FinishTest <= DateTime.UtcNow)
                {
                    return DateTime.MinValue;
                }

                return startedAttendee.StartTest.Add(timeSpan);
            }

            if (resumedAttendee.FinishTest != DateTime.MinValue && resumedAttendee.ResumeTest.Value <= resumedAttendee.FinishTest && resumedAttendee.FinishTest <= DateTime.UtcNow)
            {
                return DateTime.MinValue;
            }

            var timeSpanBeforeResume = resumedAttendee.FinishTest.Subtract(resumedAttendee.StartTest);
            var timeSpanLeft = timeSpan - timeSpanBeforeResume;

            return resumedAttendee.ResumeTest.Value.Add(timeSpanLeft);
        }
        #endregion

        #region Private Method

        /// <summary>
        /// Calls code base simulator to execute the code
        /// </summary>
        /// <param name="codeObject"></param>
        /// <returns>Result object</returns>
        private async Task<Result> ExecuteCodeAsync(Code codeObject)
        {
            //Add API KEY
            codeObject.Key = _configuration["CodeBaseSimulatorServerAPIKey"];

            var serializedCode = JsonConvert.SerializeObject(codeObject);
            var body = new StringContent(serializedCode, System.Text.Encoding.UTF8, "application/json");
            var codeBaseServer = _configuration["CodeBaseSimulatorServer"];
            var response = await _httpService.PostAsync(codeBaseServer, body);
            var content = response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Result>(content.Result);

            return result;
        }

        /// <summary>
        /// Transform Attendee's JSON formatted Answer stored in AttendeeAnswers table
        /// to reliable TestAnswer and TestConduct tables.
        /// </summary>
        /// <param name="attendeeId">Id of Test Attendee</param>
        private async Task TransformAttendeeAnswer(int attendeeId)
        {
            var attendeeAnswer = await _dbContext.AttendeeAnswers.AsNoTracking().SingleOrDefaultAsync(x => x.Id == attendeeId);
            var testAnswersList = new List<TestAnswers>();
            var testConductList = new List<DomainModel.Models.TestConduct.TestConduct>();
            if (attendeeAnswer is { Answers: { } })
            {
                var deserializedAnswer = (JsonConvert.DeserializeObject<TestAnswerAC[]>(attendeeAnswer.Answers) ?? throw new InvalidOperationException()).ToList();

                //Remove existing transformation
                var testConductExist = await _dbContext.TestConduct.AnyAsync(x => x.TestAttendeeId == attendeeId);
                if (testConductExist)
                {
                    var testConductListToRemove = await _dbContext.TestConduct.Where(x => x.TestAttendeeId == attendeeId).ToListAsync();
                    _dbContext.TestConduct.RemoveRange(testConductListToRemove);
                    await _dbContext.SaveChangesAsync();
                }

                foreach (var answer in deserializedAnswer)
                {
                    var testAnswers = new TestAnswers();

                    //Adding attempted Question to TestConduct table
                    var testConduct = new DomainModel.Models.TestConduct.TestConduct()
                    {
                        QuestionId = answer.QuestionId,
                        QuestionStatus = answer.QuestionStatus,
                        TestAttendeeId = attendeeId,
                        IsAnswered = answer.IsAnswered
                    };
                    testConductList.Add(testConduct);

                    //Adding answer to TestAnswer Table
                    if (answer.OptionChoice.Any())
                    {
                        //A question can have multiple answer
                        foreach (var option in answer.OptionChoice)
                        {
                            testAnswers = new TestAnswers()
                            {
                                AnsweredOption = option,
                                TestConduct = testConduct
                            };
                            testAnswersList.Add(testAnswers);
                        }
                    }
                    else
                    {
                        //Save answer for code snippet question
                        if (answer.Code != null)
                        {
                            testAnswers.AnsweredCodeSnippet = answer.Code.Source;
                        }

                        testAnswers.TestConduct = testConduct;
                        testAnswersList.Add(testAnswers);
                    }
                }
                await _dbContext.TestAnswers.AddRangeAsync(testAnswersList);
                await _dbContext.TestConduct.AddRangeAsync(testConductList);
                await _dbContext.SaveChangesAsync();
            }
        }

        private async Task GetTotalMarks(int testAttendeeId)
        {
            //Gets the details of the test attendee and also includes the test details taken by that test attendee
            var testAttendee = await _dbContext.TestAttendees.AsNoTracking().Include(x => x.Test).FirstAsync(x => x.Id == testAttendeeId);

            decimal correctMarks = 0;
            var noOfCorrectAttempts = 0;

            //Gets the list of questions attended by the test attendee and includes the test answers and question details corresponding to the test attendee
            var listOfQuestionsAttendedByTestAttendee = await _dbContext.TestConduct.AsNoTracking().Include(x => x.TestAnswers).Include(x => x.Question).Where(x => x.TestAttendeeId == testAttendeeId).ToListAsync();

            //Gets the Ids of questions attended by test attendee
            var attendedQuestionIds = listOfQuestionsAttendedByTestAttendee.Where(x => x.Question.QuestionType != QuestionType.Programming && x.TestAttendeeId == testAttendeeId).Select(x => x.Id).ToList();

            //Gets the testConduct details of the questions attended by test attendee which are not coding type questions
            var testConductObjectList = await _dbContext.TestConduct.Include(x => x.Question).Include(x => x.Question.SingleMultipleAnswerQuestion).ThenInclude(x => x.SingleMultipleAnswerQuestionOption).Where(x => attendedQuestionIds.Contains(x.Id) && x.TestAttendeeId == testAttendeeId).ToListAsync();

            //Gets the number of questions in the test taken by the test attendee
            var numberOfQuestionsInATest = await _dbContext.TestQuestion.AsNoTracking().CountAsync(x => x.TestId == testAttendee.TestId);

            //Calculates the full marks of the test taken by the test attendee
            var fullMarks = numberOfQuestionsInATest * testAttendee.Test.CorrectMarks;

            var testSolutionList = await _dbContext.TestCodeSolution.AsNoTracking().OrderByDescending(x => x.CreatedDateTime).Where(x => x.TestAttendeeId == testAttendeeId).ToListAsync();

            foreach (var attendedQuestion in listOfQuestionsAttendedByTestAttendee)
            {
                var isAnsweredOptionNull = false;

                //Checks the status of the question attended by the test attendee and continues with the loop if the question status is either unanswered or selected
                if (attendedQuestion.QuestionStatus == QuestionStatus.unanswered || attendedQuestion.QuestionStatus == QuestionStatus.selected)
                    continue;

                var numberOfcorrectOptionsAnsweredByTestAttendee = 0;
                var numberOfCorrectOptionsOfMultipleAnswerQuestion = 0;

                if (attendedQuestion.Question.QuestionType != QuestionType.Programming)
                {
                    var isAnsweredOptionCorrect = true;

                    //Gets the single multiple question details along with the single multiple question options for a question attended by the test attendee when it is not a coding question 
                    var testConductObject = testConductObjectList.First(x => x.QuestionId == attendedQuestion.QuestionId);

                    //Gets the options of the single and multiple-answer question attempted by the test attendee
                    var listOfOptions = testConductObject.Question.SingleMultipleAnswerQuestion.SingleMultipleAnswerQuestionOption.ToList();

                    //Calculates the number of correct options saved for the multiple-answer question attempted by the test attendee
                    if (attendedQuestion.Question.QuestionType == QuestionType.Multiple)
                        listOfOptions.ForEach(correct =>
                        {
                            numberOfCorrectOptionsOfMultipleAnswerQuestion = correct.IsAnswer ? numberOfCorrectOptionsOfMultipleAnswerQuestion + 1 : numberOfCorrectOptionsOfMultipleAnswerQuestion;
                        });

                    foreach (var answers in attendedQuestion.TestAnswers)
                    {
                        var answeredOption = answers.AnsweredOption;

                        //Checks if none of the options are marked by a test attendee for single-multiple-answer question
                        if (answeredOption == null)
                        {
                            isAnsweredOptionNull = true;
                            continue;
                        }

                        //Checks whether the option answered by the test attendee for the single-answer question is correct or not
                        if (!testConductObject.Question.SingleMultipleAnswerQuestion.SingleMultipleAnswerQuestionOption.ToList().First(x => x.Id == answeredOption).IsAnswer && attendedQuestion.Question.QuestionType == QuestionType.Single)
                        {
                            isAnsweredOptionCorrect = false;
                            break;
                        }

                        if (attendedQuestion.Question.QuestionType == QuestionType.Multiple)
                        {
                            //Calculates the number of answers given correctly for multiple-answer question by the test attendee
                            numberOfcorrectOptionsAnsweredByTestAttendee = testConductObject.Question.SingleMultipleAnswerQuestion.SingleMultipleAnswerQuestionOption.ToList().First(x => x.Id == answeredOption).IsAnswer ? numberOfcorrectOptionsAnsweredByTestAttendee + 1 : numberOfcorrectOptionsAnsweredByTestAttendee - 1;
                            //Returns true if the number of correct options given by the test attendee for the multiple-answer question attempted equals the number of correct options saved for that multiple-answer question else returns false
                            isAnsweredOptionCorrect = numberOfcorrectOptionsAnsweredByTestAttendee == numberOfCorrectOptionsOfMultipleAnswerQuestion;
                        }
                    }

                    if (isAnsweredOptionCorrect && !isAnsweredOptionNull)
                    {
                        //Add score for single-multiple answer question when correct
                        correctMarks += testAttendee.Test.CorrectMarks;
                        noOfCorrectAttempts += 1;
                    }

                    else if (!isAnsweredOptionNull)
                        //Subtract score for single-multiple answer question when incorrect
                        correctMarks -= testAttendee.Test.IncorrectMarks;
                }
                else
                {
                    //Add score from coding question attempted
                    var testSolution = testSolutionList.Where(x => x.QuestionId == attendedQuestion.QuestionId).Select(x => new { x.Score }).FirstOrDefault();
                    if (testSolution != null)
                    {
                        correctMarks += (decimal)testSolution.Score * testAttendee.Test.CorrectMarks;
                        if (correctMarks == testAttendee.Test.CorrectMarks)
                            noOfCorrectAttempts = +1;
                    }
                }
            }
            var totalMarks = correctMarks;
            totalMarks = Math.Round(totalMarks, 2);

            //Gets the report details of the the test attendee
            var report = await _dbContext.Report.SingleAsync(x => x.TestAttendeeId == testAttendeeId);

            report.TotalMarksScored = (double)totalMarks;
            report.Percentage = (report.TotalMarksScored / (double)fullMarks) * 100;
            report.Percentage = Math.Round(report.Percentage, 2);
            report.TotalCorrectAttempts = noOfCorrectAttempts;
            _dbContext.Report.Update(report);
            await _dbContext.SaveChangesAsync();
        }

        private async Task GetTimeTakenByAttendeeAsync(int attendeeId)
        {
            var report = await _dbContext.Report.FirstAsync(x => x.TestAttendeeId == attendeeId);
            var elapsedTime = await GetElapsedTimeAsync(attendeeId);
            report.TimeTakenByAttendee = (int)(elapsedTime * 60f);
            await _dbContext.SaveChangesAsync();
        }
        #endregion
    }
}