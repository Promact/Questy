﻿using Microsoft.EntityFrameworkCore;
using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.Models.TestConduct;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Promact.Trappist.DomainModel.Enum;
using Promact.Trappist.DomainModel.Models.Test;
using Promact.Trappist.DomainModel.ApplicationClasses.Reports;

namespace Promact.Trappist.Repository.Reports
{
    public class ReportRepository : IReportRepository
    {
        #region Private Members
        private readonly TrappistDbContext _dbContext;
        #endregion

        #region Constructor
        public ReportRepository(TrappistDbContext dbContext)
        {
            _dbContext = dbContext;
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
            var allTestAttendeeList = await _dbContext.TestAttendees.Where(x => x.TestId == testId).Include(y => y.Report).ToListAsync();
            var testAttendeeAnswerList = new List<TestAnswers>();
            var testQuestionList = new List<TestQuestion>();
            var allAttendeeMarksDetailsList = new List<ReportQuestionsCountAC>();
            testQuestionList = await GetTestQuestions(testId);
            var easyQuestionAttempted = 0; var hardQuestionAttempted = 0; var mediumQuestionAttempted = 0;
            var correctAttemptedQuestion = 0; var totalCorrectOptions = 0; var countOptions=0;
            foreach (var testAttendee in allTestAttendeeList)
            {
                if (testAttendee.Report != null)
                {               
                    var questionAttemptedList = await _dbContext.TestConduct.Where(x => x.TestAttendeeId == testAttendee.Id).ToListAsync();
                    var totalQuestionAttempted = questionAttemptedList.Count();
                    var testAnswersList = await GetTestAttendeeAnswers(testAttendee.Id);
                    questionAttemptedList.ForEach(x =>
                    {
                        var difficultyLevel = x.Question.DifficultyLevel;
                        if (difficultyLevel == DifficultyLevel.Easy)
                            easyQuestionAttempted += 1;
                        else
                        {
                            if (difficultyLevel == DifficultyLevel.Medium)
                                mediumQuestionAttempted += 1;
                            else
                            hardQuestionAttempted += 1;
                        }

                        if (x.Question.QuestionType == QuestionType.Single && x.QuestionStatus == QuestionStatus.answered)
                        {
                            var question = testQuestionList.FirstOrDefault(y => y.QuestionId == x.QuestionId);
                            var correctOption = question.Question.SingleMultipleAnswerQuestion.SingleMultipleAnswerQuestionOption.FirstOrDefault(z=>z.IsAnswer);
                            var givenOptionsByAttendee = testAnswersList.Where(y => y.TestConductId == x.Id).Select(y => y.AnsweredOption).ToList();
                            if (givenOptionsByAttendee.First().Value == correctOption.Id)
                                correctAttemptedQuestion += 1;
                        }
                        if (x.Question.QuestionType == QuestionType.Multiple && x.QuestionStatus==QuestionStatus.answered)
                        {
                            var question = testQuestionList.FirstOrDefault(y => y.QuestionId == x.QuestionId);
                            var Options = question.Question.SingleMultipleAnswerQuestion.SingleMultipleAnswerQuestionOption.ToList();
                            totalCorrectOptions = Options.Where(y=>y.IsAnswer).Count();
                            var givenOptionsByAttendee = testAnswersList.Where(y => y.TestConductId == x.Id).Select(y=>y.AnsweredOption).ToList();
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
                        }
                    });
                    var reportQuestions = new ReportQuestionsCountAC()
                    {
                        TestAttendeeId = testAttendee.Id,
                        EasyQuestionAttempted = easyQuestionAttempted,
                        MediumQuestionAttempted = mediumQuestionAttempted,
                        HardQuestionAttempted = hardQuestionAttempted,
                        CorrectQuestionsAttempted = correctAttemptedQuestion,
                        NoOfQuestionAttempted = totalQuestionAttempted
                    };
                    allAttendeeMarksDetailsList.Add(reportQuestions);
                    easyQuestionAttempted = mediumQuestionAttempted = hardQuestionAttempted = correctAttemptedQuestion = countOptions = 0;
                }};
            return allAttendeeMarksDetailsList;
        }
        #endregion
    }

}
