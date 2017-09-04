
using Promact.Trappist.DomainModel.Enum;
using Promact.Trappist.DomainModel.Models.Test;
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

        public int FocusLostTime { get; set; }

        public string WarningMessage { get; set; }

        public decimal CorrectMarks { get; set; }

        public decimal IncorrectMarks { get; set; }

        public bool IsPaused { get; set; }

        public bool IsLaunched { get; set; }

        public TestOrder QuestionOrder { get; set; }

        public TestOrder OptionOrder { get; set; }

        public AllowTestResume AllowTestResume { get; set; }

        public List<CategoryAC> CategoryAcList { get; set; }

        public List<TestIpAddressAC> TestIpAddress { get; set; }

        public int NumberOfTestAttendees { get; set; }

        public int NumberOfTestSections { get; set; }

        public int NumberOfTestQuestions { get; set; }

        public int TestCopiedNumber { get; set; }

        public string CreatedByUserId { get; set; }
    }
}
