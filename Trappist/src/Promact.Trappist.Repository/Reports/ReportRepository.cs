using Microsoft.EntityFrameworkCore;
using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.Models.TestConduct;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Promact.Trappist.Repository.Reports
{
    public class ReportRepository : IReportRepository
    {
        private readonly TrappistDbContext _dbContext;

        #region Constructor
        public ReportRepository(TrappistDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        public async Task<ICollection<TestAttendees>> GetAllTestAttendeesAsync(int id)
        {
            return await _dbContext.TestAttendees.Where(t => t.TestId == id).Include(x => x.Report).ToListAsync();
        }

        public async Task SetStarredCandidateAsync(int id)
        {
            var studentToBeStarred = await _dbContext.TestAttendees.Where(x => x.Id == id).FirstOrDefaultAsync();
            studentToBeStarred.StarredCandidate = studentToBeStarred.StarredCandidate ? false : true;
            await _dbContext.SaveChangesAsync();
        }

        public async Task SetAllCandidateStarredAsync(int id, bool status, List<int> idList)
        {
            for (int i = 0; i < idList.Count; i++)
            {
                var attendee = await _dbContext.TestAttendees.FindAsync(idList[i]);
                attendee.StarredCandidate = status;
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}
