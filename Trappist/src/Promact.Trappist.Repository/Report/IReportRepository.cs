using Promact.Trappist.DomainModel.Models.Question;
using Promact.Trappist.DomainModel.Models.Test;
using Promact.Trappist.DomainModel.Models.TestConduct;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Promact.Trappist.Repository.Report
{
    public interface IReportRepository
    {
        IEnumerable GetAllTestAttendees(int testId);

        Task<TestAttendees> GetTestAttendeeByIdAsync(int id);

        Task<List<TestQuestion>> GetTestQuestions(int testId);
    }
}
