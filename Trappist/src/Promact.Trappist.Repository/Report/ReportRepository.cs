using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Promact.Trappist.DomainModel.Models.TestConduct;
using Promact.Trappist.DomainModel.DbContext;
using System.Linq;
using System.Collections;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Promact.Trappist.DomainModel.Models.Test;
using Promact.Trappist.DomainModel.Models.Question;

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

        public async Task<TestAttendees> GetTestAttendeeByIdAsync(int testAttendeeId)
        {
            var testAttendee = await _dbContext.TestAttendees.Include(x => x.Test).FirstOrDefaultAsync(x => x.Id == testAttendeeId);
            return testAttendee;
        }

        public async Task<List<TestQuestion>> GetTestQuestions(int testId)
        {
            return await _dbContext.TestQuestion.Include(x => x.Question).Include(x => x.Question.SingleMultipleAnswerQuestion).ThenInclude(x => x.SingleMultipleAnswerQuestionOption).Where(x => x.TestId == testId).ToListAsync();
        }
    }
}