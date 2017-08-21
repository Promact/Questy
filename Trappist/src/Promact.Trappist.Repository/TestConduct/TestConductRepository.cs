using CodeBaseSimulator.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Promact.Trappist.DomainModel.ApplicationClasses.CodeSnippet;
using Promact.Trappist.DomainModel.ApplicationClasses.TestConduct;
using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.Enum;
using Promact.Trappist.DomainModel.Models.Report;
using Promact.Trappist.DomainModel.Models.TestConduct;
using Promact.Trappist.DomainModel.Models.TestLogs;
using Promact.Trappist.Repository.Questions;
using Promact.Trappist.Repository.Tests;
using Promact.Trappist.Utility.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Promact.Trappist.Repository.TestConduct
{
    public class TestConductRepository : ITestConductRepository
    {
        #region Private Variables
        #region Dependencies
        private readonly TrappistDbContext _dbContext;
        private readonly ITestsRepository _testRepository;
        private readonly HttpClient client;
        private readonly IQuestionRepository _questionRepository;
        private readonly IConfiguration _configuration;
        private readonly IStringConstants _stringConstants;
        #endregion
        #endregion

        #region Constructor
        public TestConductRepository(TrappistDbContext dbContext, ITestsRepository testRepository, IConfiguration configuration, IQuestionRepository questionRepository, IStringConstants stringConstants)
        {
            _dbContext = dbContext;
            _testRepository = testRepository;
            _questionRepository = questionRepository;
            _configuration = configuration;
            _stringConstants = stringConstants;
            client = new HttpClient();
        }
        #endregion

        #region Public Method
        public async Task RegisterTestAttendeesAsync(TestAttendees testAttendee, string magicString)
        {
            await _dbContext.TestAttendees.AddAsync(testAttendee);
            await _dbContext.SaveChangesAsync();
            var testLogsObject = new TestLogs();
            if (testLogsObject != null)
            {
                testLogsObject.TestAttendeeId = testAttendee.Id;
                testLogsObject.VisitTestLink = DateTime.UtcNow;
                testLogsObject.FillRegistrationForm = DateTime.UtcNow;
                await _dbContext.TestLogs.AddAsync(testLogsObject);
                await _dbContext.SaveChangesAsync();
            }
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

        public async Task<bool> IsTestLinkExistForTestConductionAsync(string magicString, string userIp)
        {
            var testObject = await _dbContext.Test.FirstOrDefaultAsync(x => x.Link == magicString);
            var currentDate = DateTime.UtcNow;
            await _dbContext.TestIpAddresses.Where(x => x.TestId == testObject.Id).ToListAsync();
            // if Test is not paused and current date is not greater than EndDate and machine IP address is in the list of test ip addresses of  then it returns true and test link exist otherwise link does not exist
            if (testObject.TestIpAddress != null)
                return testObject != null && DateTime.Compare(currentDate, testObject.EndDate) < 0 && !testObject.IsPaused && testObject.TestIpAddress.Any(x => x.IpAddress == userIp) && testObject.IsLaunched;
            else return testObject != null && DateTime.Compare(currentDate, testObject.EndDate) < 0 && !testObject.IsPaused && testObject.IsLaunched;
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
            if (attendeeAnswer.Answers != null)
            {
                var deserializedAnswer = JsonConvert.DeserializeObject<TestAnswerAC[]>(attendeeAnswer.Answers);
                if (deserializedAnswer[0] != null)
                {
                    return !(deserializedAnswer.Where(x => x.QuestionId == answer.QuestionId && x.QuestionStatus == QuestionStatus.answered).Count() > 0);
                }
            }
            return true;
        }

        public async Task<ICollection<TestAnswerAC>> GetAnswerAsync(int attendeeId)
        {
            var attendee = await _dbContext.AttendeeAnswers.FindAsync(attendeeId);

            if (attendee == null || attendee.Answers == null)
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
                await _dbContext.Report.AddAsync(report);
            }
            report.TestStatus = testStatus;
            await _dbContext.SaveChangesAsync();
            if (testStatus != TestStatus.AllCandidates)
            {
                //Begin transformation
                await TransformAttendeeAnswer(attendeeId);
                await GetTotalMarks(attendeeId);
            }
            var testLogs = await _dbContext.TestLogs.FirstOrDefaultAsync(x => x.TestAttendeeId == attendeeId);
            testLogs.FinishTest = DateTime.UtcNow;
            _dbContext.TestLogs.Update(testLogs);
            await _dbContext.SaveChangesAsync();
            await GetTimeTakenByAttendeeAsync(attendeeId);
        }

        public async Task<TestStatus> GetAttendeeTestStatusAsync(int attendeeId)
        {
            var report = await _dbContext.Report.OrderBy(x => x.TestAttendeeId).FirstOrDefaultAsync(x => x.TestAttendeeId == attendeeId);
            if (report == null)
                return TestStatus.AllCandidates;

            return report.TestStatus;
        }

        public async Task<bool> IsTestInValidDateWindow(string testLink, bool isPreview)
        {
            if (isPreview)
                return true;
            else
            {
                var currentDate = DateTime.UtcNow;
                var startTime = await _dbContext.Test.Where(x => x.Link == testLink).Select(x => x.StartDate).SingleAsync();
                var endTime = await _dbContext.Test.Where(x => x.Link == testLink).Select(x => x.EndDate).SingleAsync();
                return currentDate.CompareTo(startTime) >= 0 && currentDate.CompareTo(endTime) <= 0;
            }
        }

        public async Task<CodeResponse> ExecuteCodeSnippetAsync(int attendeeId, TestAnswerAC testAnswer)
        {
            var allTestCasePassed = true;
            var countPassedTest = 0;
            var errorEncounter = false;
            var errorMessage = "";
            var score = 0d;
            var code = testAnswer.Code;
            var codeResponse = new CodeResponse();
            var testCases = new List<DomainModel.Models.Question.CodeSnippetQuestionTestCases>();
            var results = new List<Result>();
            var testCaseResults = new List<TestCaseResult>();

            await AddAnswerAsync(attendeeId, testAnswer);

            var testCaseChecks = await _dbContext.CodeSnippetQuestion.SingleOrDefaultAsync(x => x.Id == testAnswer.QuestionId);

            var completeTestCases = await _dbContext.CodeSnippetQuestionTestCases.Where(x => x.CodeSnippetQuestionId == testAnswer.QuestionId).ToListAsync();

            //Filter Test Cases
            testCases.AddRange(completeTestCases.Where(x => x.TestCaseType == TestCaseType.Default).ToList());
            if (testCaseChecks.RunBasicTestCase)
                testCases.AddRange(completeTestCases.Where(x => x.TestCaseType == TestCaseType.Basic).ToList());
            if (testCaseChecks.RunCornerTestCase)
                testCases.AddRange(completeTestCases.Where(x => x.TestCaseType == TestCaseType.Corner).ToList());
            if (testCaseChecks.RunNecessaryTestCase)
                testCases.AddRange(completeTestCases.Where(x => x.TestCaseType == TestCaseType.Necessary).ToList());
            //End of filter

            foreach (var testCase in testCases)
            {
                code.Input = testCase.TestCaseInput;
                var result = await ExecuteCodeAsync(code);

                if (result.CompilerOutput != "")//EXITCODE if code run fails
                {
                    errorEncounter = true;
                    errorMessage = result.CompilerOutput;
                    break;
                }

                //Trim newline character 
                result.Output = result.Output.TrimEnd(new char[] { '\r', '\n' });

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
            foreach(var testCaseResult in testCaseResults)
            {
                testCaseResult.TestCodeSolution = codeSolution; 
            }
            await _dbContext.TestCaseResult.AddRangeAsync(testCaseResults);
            await _dbContext.SaveChangesAsync();

            codeResponse.ErrorOccurred = false;
            if (allTestCasePassed && !errorEncounter)
            {
                codeResponse.Message = _stringConstants.AllTestCasePassed;
            }
            else if (!errorEncounter)
            {
                if (countPassedTest > 0)
                {
                    codeResponse.Message = _stringConstants.SomeTestCasePassed;
                }
                else
                {
                    codeResponse.Message = _stringConstants.NoTestCasePassed;
                }
            }
            else
            {
                codeResponse.ErrorOccurred = true;
                codeResponse.Error = errorMessage;
            }

            return codeResponse;
        }

        public async Task AddTestLogsAsync(int attendeeId, bool isCloseWindow, bool isConnectionLoss, bool isTestResume)
        {
            var testLogs = await _dbContext.TestLogs.FirstOrDefaultAsync(x => x.TestAttendeeId == attendeeId);
            if (isCloseWindow)
                testLogs.CloseWindowWithoutFinishingTest = DateTime.UtcNow;
            else if (isConnectionLoss)
                testLogs.DisconnectedFromServer = DateTime.UtcNow;
            else if (isTestResume)
                testLogs.ResumeTest = DateTime.UtcNow;
            else
                testLogs.AwayFromTestWindow = DateTime.UtcNow;
            _dbContext.TestLogs.Update(testLogs);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<int> GetTestSummaryDetailsAsync(string testLink)
        {
            var testObject = await _dbContext.Test.Where(x => x.Link == testLink)
                    .Include(x => x.TestQuestion).Include(x => x.TestAttendees).ToListAsync();
            if (testObject.Any())
            {
                var testSummaryDetails = testObject.First();
                var totalNumberOfQuestions = testSummaryDetails.TestQuestion.Count();
                return totalNumberOfQuestions;
            }
            else
                return 0;
        }
        #endregion

        #region Private Method

        /// <summary>
        /// Calls code base simulator to execute the code
        /// </summary>
        /// <param name="code">Code object</param>
        /// <returns>Result object</returns>
        private async Task<Result> ExecuteCodeAsync(Code codeObject)
        {
            //Add API KEY
            codeObject.Key = _configuration["CodeBaseSimulatorServerAPIKey"];

            var serializedCode = JsonConvert.SerializeObject(codeObject);
            var body = new StringContent(serializedCode, System.Text.Encoding.UTF8, "application/json");
            var CodeBaseServer = _configuration["CodeBaseSimulatorServer"];
            var response = await client.PostAsync(CodeBaseServer, body);
            var content = response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Result>(content.Result);

            return result;
        }

        /// <summary>
        /// Transform Attendee's JSON formatted Answer stored in AttendeeAnswers table
        /// to reliable TestAnswer and TestConduct tables.
        /// After completion of transformation Attendee's record from AttendeeAnswer is removed.
        /// </summary>
        /// <param name="attendeeId">Id of Test Attendee</param>
        private async Task TransformAttendeeAnswer(int attendeeId)
        {
            var attendeeAnswer = await _dbContext.AttendeeAnswers.SingleOrDefaultAsync(x => x.Id == attendeeId);
            //var isTestConductExist = await _dbContext.TestConduct.AnyAsync(x => x.TestAttendeeId == attendeeId);

            if (attendeeAnswer != null)
            {
                if (attendeeAnswer.Answers != null)
                {
                    var deserializedAnswer = JsonConvert.DeserializeObject<TestAnswerAC[]>(attendeeAnswer.Answers).ToList();

                    foreach (var answer in deserializedAnswer)
                    {
                        var testAnswers = new TestAnswers();
                        //Adding attempted Question to TestConduct table
                        if (!await _dbContext.TestConduct.AnyAsync(x => x.QuestionId == answer.QuestionId && x.TestAttendeeId == attendeeId))
                        {
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
                                foreach (var option in answer.OptionChoice)
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
                                if (answer.Code != null)
                                {
                                    testAnswers.AnsweredCodeSnippet = answer.Code.Source;
                                }

                                testAnswers.TestConduct = testConduct;
                                await _dbContext.TestAnswers.AddAsync(testAnswers);
                                await _dbContext.SaveChangesAsync();
                            }
                        }
                    }
                }
            }
        }

        private async Task GetTotalMarks(int testAttendeeId)
        {
            var testAttendee = await _dbContext.TestAttendees.Include(x => x.Test).FirstOrDefaultAsync(x => x.Id == testAttendeeId);
            decimal correctMarks = 0;
            decimal fullMarks = 0;
            decimal totalMarks = 0;
            var listOfQuestionsAttendedByTestAttendee = await _dbContext.TestConduct.Include(x => x.TestAnswers).Where(x => x.TestAttendeeId == testAttendeeId).ToListAsync();
            var numberOfQuestionsInATest = await _dbContext.TestQuestion.Where(x => x.TestId == testAttendee.TestId).ToListAsync();
            var totalNumberOfQuestions = numberOfQuestionsInATest.Count();
            fullMarks = totalNumberOfQuestions * testAttendee.Test.CorrectMarks;

            foreach (var attendedQuestion in listOfQuestionsAttendedByTestAttendee)
            {
                if (attendedQuestion.QuestionStatus != QuestionStatus.answered)
                {
                    continue;
                }

                var count = 0;
                var correctOption = 0;

                var question = await _questionRepository.GetQuestionByIdAsync(attendedQuestion.QuestionId);

                if (question.Question.QuestionType != QuestionType.Programming)
                {
                    bool isAnsweredOptionCorrect = true;
                    var noOfOptions = question.SingleMultipleAnswerQuestion.SingleMultipleAnswerQuestionOption;
                    if (question.Question.QuestionType == QuestionType.Multiple)
                    {
                        foreach (var correct in noOfOptions)
                        {
                            if (correct.IsAnswer)
                                correctOption = correctOption + 1;
                        }
                    }

                    foreach (var answers in attendedQuestion.TestAnswers)
                    {
                        var answeredOption = answers.AnsweredOption;

                        if (!question.SingleMultipleAnswerQuestion.SingleMultipleAnswerQuestionOption.Find(x => x.Id == answeredOption).IsAnswer && question.Question.QuestionType == QuestionType.Single)
                        {
                            isAnsweredOptionCorrect = false;
                            break;
                        }
                        else if (question.Question.QuestionType == QuestionType.Multiple)
                        {
                            if (question.SingleMultipleAnswerQuestion.SingleMultipleAnswerQuestionOption.Find(x => x.Id == answeredOption).IsAnswer)
                                count = count + 1;
                            if (!question.SingleMultipleAnswerQuestion.SingleMultipleAnswerQuestionOption.Find(x => x.Id == answeredOption).IsAnswer)
                                count = count - 1;

                            isAnsweredOptionCorrect = count == correctOption;
                        }
                    }

                    if (isAnsweredOptionCorrect)
                    {
                        correctMarks += testAttendee.Test.CorrectMarks;
                    }
                    else
                    {
                        correctMarks -= testAttendee.Test.IncorrectMarks;
                    }
                }
                else
                {
                    //Add score from coding question attempted
                    correctMarks += (decimal)await _dbContext.TestCodeSolution.Where(x => x.TestAttendeeId == testAttendeeId && x.QuestionId == attendedQuestion.QuestionId).MaxAsync(x => x.Score) * testAttendee.Test.CorrectMarks;
                }
            }
            totalMarks = correctMarks;
            totalMarks = Math.Round(totalMarks, 2);
            var report = await _dbContext.Report.SingleOrDefaultAsync(x => x.TestAttendeeId == testAttendeeId);
            report.TotalMarksScored = (double)totalMarks;
            report.Percentage = (report.TotalMarksScored / (double)fullMarks) * 100;
            report.Percentage = Math.Round(report.Percentage, 2);
            _dbContext.Report.Update(report);
            await _dbContext.SaveChangesAsync();
        }

        private async Task GetTimeTakenByAttendeeAsync(int attendeeId)
        {
            var testLogs = await _dbContext.TestLogs.FirstOrDefaultAsync(x => x.TestAttendeeId == attendeeId);
            var report = await _dbContext.Report.FirstOrDefaultAsync(x => x.TestAttendeeId == attendeeId);
            DateTime date = testLogs.StartTest;

            int StartTestTimeInSeconds = (date.Hour * 60 * 60) + date.Minute * 60 + date.Second;
            DateTime elapsedTime = testLogs.FinishTest;
            int FinishTestTimeInSeconds = (elapsedTime.Hour * 60 * 60) + elapsedTime.Minute * 60 + elapsedTime.Second;
            report.TimeTakenByAttendee = FinishTestTimeInSeconds - StartTestTimeInSeconds;

            _dbContext.Report.Update(report);
            await _dbContext.SaveChangesAsync();
        }


        public async Task<List<TestLogs>> GetTestLogsAsync()
        {
            return await _dbContext.TestLogs.ToListAsync();
        }
        #endregion
    }
}