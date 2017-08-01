using Microsoft.EntityFrameworkCore;
using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.Models.TestConduct;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Promact.Trappist.DomainModel.Enum;
using Promact.Trappist.DomainModel.Models.Test;

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
            var testAttendee = await _dbContext.TestAttendees.Include(x => x.Test).Include(x => x.Report).FirstOrDefaultAsync(x => x.Id == testAttendeeId);
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

            var attendeeList = await _dbContext.TestAttendees.Include(x => x.Report).Where(x => x.TestId == testAttendee.TestId).ToListAsync();
            double noOfScores = attendeeList.Count();
            var attendee = await _dbContext.Report.Where(x => x.TestAttendeeId == testAttendeeId).FirstOrDefaultAsync();
            double studentPercentile;

            var marksList = await _dbContext.Report.Where(x => x.TestAttendee.TestId == testAttendee.TestId).OrderBy(x => x.TotalMarksScored).ToListAsync();
            var rank = marksList.IndexOf(attendee) + 1;

            double percentile = rank / noOfScores;
            studentPercentile = percentile * 100;
            return studentPercentile;
        }
        #endregion
    }
}
