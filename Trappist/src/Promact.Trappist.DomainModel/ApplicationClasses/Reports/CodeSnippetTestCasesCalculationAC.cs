using Promact.Trappist.DomainModel.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Promact.Trappist.DomainModel.ApplicationClasses.Reports
{
    public class CodeSnippetTestCasesCalculationAC
    {
        public string TestCaseName { get; set; }
        public TestCaseType TestCaseType { get; set; }
        public string TestCaseInput { get; set; }
        public string ExpectedOutput { get; set; }
        public double TestCaseMarks { get; set; }
        public long Processing { get; set; }
        public long Memory { get; set; }
        public string ActualOutput { get; set; }
        public bool IsTestCasePassing { get; set; }
    }
}
