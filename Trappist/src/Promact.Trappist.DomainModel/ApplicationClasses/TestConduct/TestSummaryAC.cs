using Promact.Trappist.DomainModel.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Promact.Trappist.DomainModel.ApplicationClasses.TestConduct
{
    public class TestSummaryAC
    {
        public DateTime? TimeLeft { get; set; }
        public int TotalNumberOfQuestions { get; set; }
        public int AttemptedQuestions { get; set; }
        public int UnAttemptedQuestions { get; set; }
        public int ReviewedQuestions { get; set; }
        public AllowTestResume ResumeType { get; set; }
    }
}
