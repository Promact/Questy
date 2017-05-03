﻿using Microsoft.EntityFrameworkCore;
using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.Models.TestConduct;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Promact.Trappist.DomainModel.Enum;

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
        public async Task<IEnumerable<TestAttendees>> GetAllTestAttendeesAsync(int id)
        {
            var result = await _dbContext.TestAttendees.Where(t => t.TestId == id).Include(x => x.Report).ToListAsync();
            return result;
        }

        public async Task SetStarredCandidateAsync(int id)
        {
            var studentToBeStarred = await _dbContext.TestAttendees.FindAsync(id);
            studentToBeStarred.StarredCandidate = studentToBeStarred.StarredCandidate ? false : true;
            await _dbContext.SaveChangesAsync();
        }

        public async Task SetAllCandidateStarredAsync(bool status, int selectedTestStatus, string searchString)
        {
            if(searchString == null)
            {
                searchString = "";
            }
            var attendeeList = await _dbContext.TestAttendees.Where(x => x.Report.TestStatus == (TestStatus)selectedTestStatus 
            && (x.FirstName.Contains(searchString)
            || x.LastName.Contains(searchString)
            || x.Email.Contains(searchString)
            )).ToListAsync();

            attendeeList.ForEach(x => x.StarredCandidate = status);

            await _dbContext.SaveChangesAsync();
        }
        #endregion
    }
}
