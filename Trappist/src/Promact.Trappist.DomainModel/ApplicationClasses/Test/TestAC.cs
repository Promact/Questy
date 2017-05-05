
using Promact.Trappist.DomainModel.Enum;
using System;
using System.Collections.Generic;

namespace Promact.Trappist.DomainModel.ApplicationClasses.Test
{
    public class TestAC
    {
        public int Id { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public string TestName { get; set; }

        public string Link { get; set; }

        public BrowserTolerance BrowserTolerance { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Duration { get; set; }

        public int? WarningTime { get; set; }

        public string FromIpAddress { get; set; }

        public string ToIpAddress { get; set; }

        public string WarningMessage { get; set; }

        public decimal CorrectMarks { get; set; }

        public decimal IncorrectMarks { get; set; }

        public TestOrder QuestionOrder { get; set; }

        public TestOrder OptionOrder { get; set; }

        public AllowTestResume AllowTestResume { get; set; }

        public List<CategoryAC> CategoryAcList { get; set; }

        public int NumberOfTestAttendees { get; set; }

        public int NumberOfTestSections { get; set; }

        public int NumberOfTestQuestions { get; set; }

        public string CreatedByUserId { get; set; }
    }
}
