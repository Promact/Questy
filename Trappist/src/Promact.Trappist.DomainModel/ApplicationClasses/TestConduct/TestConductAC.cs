using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using Promact.Trappist.DomainModel.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Promact.Trappist.DomainModel.ApplicationClasses.TestConduct
{
    public class TestConductAC
    {
        public QuestionAC Question { get; set; }

        public QuestionStatus QuestionStatus { get; set; }
    }
}
