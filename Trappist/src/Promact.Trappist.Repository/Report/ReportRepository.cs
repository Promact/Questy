using Microsoft.EntityFrameworkCore;
using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.Models.Test;
using Promact.Trappist.DomainModel.Models.TestConduct;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Trappist.Repository.Report
{
    public class ReportRepository : IReportRepository
    {
        private readonly TrappistDbContext _dbContext;

        public ReportRepository(TrappistDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable GetAllTestAttendees(int testId)
        {
            return _dbContext.TestAttendees.Where(x => x.TestId == testId).ToList<TestAttendees>();

        }

        public async Task<TestAttendees> GetTestAttendeeDetailsByIdAsync(int testAttendeeId)
        {
            var testAttendee = await _dbContext.TestAttendees.Include(x => x.Test).Include(x=>x.TestLogs).Include(x=>x.Report).FirstOrDefaultAsync(x => x.Id == testAttendeeId);
            return testAttendee;
        }

        public async Task<List<TestQuestion>> GetTestQuestions(int testId)
        {
            return await _dbContext.TestQuestion.Include(x => x.Question).Include(x => x.Question.SingleMultipleAnswerQuestion).ThenInclude(x => x.SingleMultipleAnswerQuestionOption).Where(x => x.TestId == testId).ToListAsync();
        }

        public async Task<List<TestAnswers>> GetTestAttendeeAnswers(int testAttendeeId)
        {
            var testAttendeeQuestionList =await  _dbContext.TestConduct.Where(x => x.TestAttendeeId == testAttendeeId).ToListAsync();
            var testAttendeeFullAnswerList = new List<TestAnswers>();

            foreach(DomainModel.Models.TestConduct.TestConduct testAttendeeQuestion in testAttendeeQuestionList)
            {
                var testAttendeeAnswerList = await _dbContext.TestAnswers.Where(x => x.TestConductId == testAttendeeQuestion.Id).ToListAsync();
                testAttendeeFullAnswerList.AddRange(testAttendeeAnswerList);
            }
            return testAttendeeFullAnswerList;
        }
    }
}