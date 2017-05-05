using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using Promact.Trappist.DomainModel.Enum;

namespace Promact.Trappist.DomainModel.ApplicationClasses.TestConduct
{
    public class TestConductAC
    {
        public QuestionAC Question { get; set; }

        public QuestionStatus QuestionStatus { get; set; }
    }
}
