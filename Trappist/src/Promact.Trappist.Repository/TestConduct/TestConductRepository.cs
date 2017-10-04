using AutoMapper;
using CodeBaseSimulator.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Promact.Trappist.DomainModel.ApplicationClasses;
using Promact.Trappist.DomainModel.ApplicationClasses.CodeSnippet;
using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using Promact.Trappist.DomainModel.ApplicationClasses.TestConduct;
using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.Enum;
using Promact.Trappist.DomainModel.Models.Question;
using Promact.Trappist.DomainModel.Models.Report;
using Promact.Trappist.DomainModel.Models.Test;
using Promact.Trappist.DomainModel.Models.TestConduct;
using Promact.Trappist.DomainModel.Models.TestLogs;
using Promact.Trappist.Repository.Questions;
using Promact.Trappist.Repository.Tests;
using Promact.Trappist.Utility.Constants;
using Promact.Trappist.Utility.HttpUtil;
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
        private readonly IQuestionRepository _questionRepository;
        private readonly IConfiguration _configuration;
        private readonly IStringConstants _stringConstants;
        private readonly IHttpService _httpService;
        #endregion
        #endregion

        #region Constructor
        public TestConductRepository(TrappistDbContext dbContext
            , ITestsRepository testRepository
            , IConfiguration configuration
            , IQuestionRepository questionRepository
            , IStringConstants stringConstants
            , IHttpService httpService)
        {
            _dbContext = dbContext;
            _testRepository = testRepository;
            _questionRepository = questionRepository;
            _configuration = configuration;
            _stringConstants = stringConstants;
            _httpService = httpService;
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
            var testObject = await _dbContext.Test.Where(x => x.Link == magicString).Include(x => x.TestIpAddress).FirstOrDefaultAsync<Test>();
            var currentDate = DateTime.UtcNow;
            // if Test is not paused and current date is not greater than EndDate and machine IP address is in the list of test ip addresses of  then it returns true and test link exist otherwise link does not exist
            if (testObject != null && testObject.TestIpAddress.Count != 0)
                return testObject != null && DateTime.Compare(currentDate, testObject.StartDate) >= 0 && DateTime.Compare(currentDate, testObject.EndDate) < 0 && !testObject.IsPaused && testObject.TestIpAddress.Any(x => x.IpAddress.Equals(userIp)) && testObject.IsLaunched;
            else return testObject != null && DateTime.Compare(currentDate, testObject.StartDate) >= 0 && DateTime.Compare(currentDate, testObject.EndDate) < 0 && !testObject.IsPaused && testObject.IsLaunched;
        }

        public async Task AddAnswerAsync(int attendeeId, TestAnswerAC answer)
        {
            if (await _dbContext.AttendeeAnswers.AsNoTracking().AnyAsync(x => x.Id == attendeeId))
            {
                var attendeeAnswer = await _dbContext.AttendeeAnswers.Where(x => x.Id == attendeeId).SingleAsync();
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

        public async Task<ICollection<TestAnswerAC>> GetAnswerAsync(int attendeeId)
        {
            var attendee = await _dbContext.AttendeeAnswers.AsNoTracking().SingleOrDefaultAsync(x => x.Id == attendeeId);

            if (attendee == null || attendee.Answers == null)
                return null;

            var deserializedAttendeeAnswers = JsonConvert.DeserializeObject<ICollection<TestAnswerAC>>(attendee.Answers);
            return deserializedAttendeeAnswers;
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

        public async Task SetElapsedTimeAsync(int attendeeId, long seconds)
        {
            var attendee = await _dbContext.AttendeeAnswers.FindAsync(attendeeId);
            if (attendee != null)
            {
                attendee.TimeElapsed = ((double)seconds / 60d);
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

            if (attendeeAnswer != null)
                return attendeeAnswer.TimeElapsed;
            else
                return 0.0;
        }

        public async Task<TestAttendees> SetAttendeeTestStatusAsync(int attendeeId, TestStatus testStatus)
        {
            var testAttendee = await _dbContext.TestAttendees.Where(x => x.Id == attendeeId).Include(x => x.Report).FirstOrDefaultAsync();
            if (testAttendee.Report == null)
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
            var testLogs = await _dbContext.TestLogs.FirstOrDefaultAsync(x => x.TestAttendeeId == attendeeId);
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
            foreach (var testCaseResult in testCaseResults)
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

            //Add answer to the database
            testAnswer.Code.Result = codeResponse.ErrorOccurred ? codeResponse.Error : codeResponse.Message;
            await AddAnswerAsync(attendeeId, testAnswer);

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
            else
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
            var attendeeBrowserToleranceValue = await _dbContext.TestAttendees.FirstOrDefaultAsync(x => x.Id == attendeeId);
            attendeeBrowserToleranceValue.AttendeeBrowserToleranceCount = attendeeBrowserToleranceCount;
            _dbContext.TestAttendees.Update(attendeeBrowserToleranceValue);
            await _dbContext.SaveChangesAsync();
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
            var response = await _httpService.PostAsync(CodeBaseServer, body);
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
            if (attendeeAnswer != null)
            {
                if (attendeeAnswer.Answers != null)
                {
                    var deserializedAnswer = JsonConvert.DeserializeObject<TestAnswerAC[]>(attendeeAnswer.Answers).ToList();

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

                        var testConduct = new DomainModel.Models.TestConduct.TestConduct();
                        //Adding attempted Question to TestConduct table
                        testConduct = new DomainModel.Models.TestConduct.TestConduct()
                        {
                            QuestionId = answer.QuestionId,
                            QuestionStatus = answer.QuestionStatus,
                            TestAttendeeId = attendeeId,
                            IsAnswered = answer.IsAnswered
                        };
                        testConductList.Add(testConduct);

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
        }

        private async Task GetTotalMarks(int testAttendeeId)
        {
            //Gets the details of the test attendee and also includes the test details taken by that test attendee
            var testAttendee = await _dbContext.TestAttendees.AsNoTracking().Include(x => x.Test).FirstOrDefaultAsync(x => x.Id == testAttendeeId);

            decimal correctMarks = 0;
            decimal fullMarks = 0;
            decimal totalMarks = 0;
            var noOfCorrectAttempts = 0;
            bool isAnsweredOptionNull = false;

            //Gets the list of questions attended by the test attendee and includes the test answers, question, single-multiple-answer question and single-multiple-answer options corresponding to the test attendee
            var listOfQuestionsAttendedByTestAttendee = await _dbContext.TestConduct.AsNoTracking().Include(x => x.TestAnswers).Include(x => x.Question).ThenInclude(x => x.SingleMultipleAnswerQuestion).ThenInclude(x => x.SingleMultipleAnswerQuestionOption).Where(x => x.TestAttendeeId == testAttendeeId).ToListAsync();

            //Gets the number of questions in the test taken by the test attendee
            var numberOfQuestionsInATest = await _dbContext.TestQuestion.AsNoTracking().CountAsync(x => x.TestId == testAttendee.TestId);

            //Calculates the full marks of the test taken by the test attendee
            fullMarks = numberOfQuestionsInATest * testAttendee.Test.CorrectMarks;

            foreach (var attendedQuestion in listOfQuestionsAttendedByTestAttendee)
            {
                isAnsweredOptionNull = false;

                //Checks the status of the question attended by the test attendee and continues with the loop if the question status is either unanswered or selected
                if (attendedQuestion.QuestionStatus == QuestionStatus.unanswered || attendedQuestion.QuestionStatus == QuestionStatus.selected)
                    continue;

                var numberOfcorrectOptionsAnsweredByTestAttendee = 0;
                var numberOfCorrectOptionsOfMultipleAnswerQuestion = 0;

                if (attendedQuestion.Question.QuestionType != QuestionType.Programming)
                {
                    var isAnsweredOptionCorrect = true;

                    //Gets the options of the single and multiple-answer question attempted by the test attendee
                    var listOfOptions = attendedQuestion.Question.SingleMultipleAnswerQuestion.SingleMultipleAnswerQuestionOption.ToList();

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
                        if (!attendedQuestion.Question.SingleMultipleAnswerQuestion.SingleMultipleAnswerQuestionOption.ToList().Find(x => x.Id == answeredOption).IsAnswer && attendedQuestion.Question.QuestionType == QuestionType.Single)
                        {
                            isAnsweredOptionCorrect = false;
                            break;
                        }
                        else if (attendedQuestion.Question.QuestionType == QuestionType.Multiple)
                        {
                            //Calculates the number of answers given correctly for multiple-answer question by the test attendee
                            numberOfcorrectOptionsAnsweredByTestAttendee = attendedQuestion.Question.SingleMultipleAnswerQuestion.SingleMultipleAnswerQuestionOption.ToList().Find(x => x.Id == answeredOption).IsAnswer ? numberOfcorrectOptionsAnsweredByTestAttendee = numberOfcorrectOptionsAnsweredByTestAttendee + 1 : numberOfcorrectOptionsAnsweredByTestAttendee - 1;
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
                    var testSolution = await _dbContext.TestCodeSolution.AsNoTracking().OrderByDescending(x => x.CreatedDateTime).Where(x => x.TestAttendeeId == testAttendeeId && x.QuestionId == attendedQuestion.QuestionId).Select(x => new { x.Score }).FirstOrDefaultAsync();
                    if (testSolution != null)
                    {
                        correctMarks += (decimal)testSolution.Score * testAttendee.Test.CorrectMarks;
                        if (correctMarks == testAttendee.Test.CorrectMarks)
                            noOfCorrectAttempts = +1;
                    }
                }
            }
            totalMarks = correctMarks;
            totalMarks = Math.Round(totalMarks, 2);

            //Gets the report details of the the test attendee
            var report = await _dbContext.Report.SingleOrDefaultAsync(x => x.TestAttendeeId == testAttendeeId);

            report.TotalMarksScored = (double)totalMarks;
            report.Percentage = (report.TotalMarksScored / (double)fullMarks) * 100;
            report.Percentage = Math.Round(report.Percentage, 2);
            report.TotalCorrectAttempts = noOfCorrectAttempts;
            _dbContext.Report.Update(report);
            await _dbContext.SaveChangesAsync();
        }

        private async Task GetTimeTakenByAttendeeAsync(int attendeeId)
        {
            var testLogs = await _dbContext.TestLogs.AsNoTracking().FirstOrDefaultAsync(x => x.TestAttendeeId == attendeeId);
            var report = await _dbContext.Report.FirstOrDefaultAsync(x => x.TestAttendeeId == attendeeId);
            DateTime date = testLogs.StartTest;

            int StartTestTimeInSeconds = (date.Hour * 60 * 60) + date.Minute * 60 + date.Second;
            DateTime elapsedTime = testLogs.FinishTest;
            int FinishTestTimeInSeconds = (elapsedTime.Hour * 60 * 60) + elapsedTime.Minute * 60 + elapsedTime.Second;
            report.TimeTakenByAttendee = FinishTestTimeInSeconds - StartTestTimeInSeconds;

            _dbContext.Report.Update(report);
            await _dbContext.SaveChangesAsync();
        }
        #endregion
    }
}