using Microsoft.EntityFrameworkCore;
using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.Models.TestConduct;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
           var result= await _dbContext.TestAttendees.Where(t => t.TestId == id).ToListAsync();
            return result;
        }
    }
}
