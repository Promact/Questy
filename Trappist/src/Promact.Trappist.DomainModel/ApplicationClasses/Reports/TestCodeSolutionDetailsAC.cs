using Promact.Trappist.DomainModel.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Promact.Trappist.DomainModel.ApplicationClasses.Reports
{
    public class TestCodeSolutionDetailsAC
    {
        public ProgrammingLanguage Language { get; set; }
        public int TotalNumberOfAttempts { get; set; }
        public int NumberOfSuccessfulAttempts { get; set; }
        public string CodeSolution { get; set; }
    }
}
