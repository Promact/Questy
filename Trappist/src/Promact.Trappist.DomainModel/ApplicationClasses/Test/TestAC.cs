using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using Promact.Trappist.DomainModel.Models.Test;
using System;
using System.Collections.Generic;

namespace Promact.Trappist.DomainModel.ApplicationClasses.Test
{
    public class TestAC
    {
        public int Id { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public string TestName { get; set; }
        public string Link { get; set; }
        public int BrowserTolerance { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }
        public int WarningTime { get; set; }
        public string FromIpAddress { get; set; }
        public string ToIpAddress { get; set; }
        public string WarningMessage { get; set; }
        public decimal CorrectMarks { get; set; }
        public decimal IncorrectMarks { get; set; }
        public List<CategoryAC> CategoryACList { get; set; }
        public List<QuestionAC> QuestionACList { get; set; }

    }
}
