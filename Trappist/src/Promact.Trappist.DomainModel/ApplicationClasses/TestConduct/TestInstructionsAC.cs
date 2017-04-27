using System.Collections.Generic;

namespace Promact.Trappist.DomainModel.ApplicationClasses.TestConduct
{
    public class TestInstructionsAC
    {
        public int Duration { get; set; }
        public decimal CorrectMarks { get; set; }
        public decimal IncorrectMarks { get; set; }
        public int TotalNumberOfQuestions { get; set; }
        public List<string> CategoryNameList { get; set; }
    }
}
