using Promact.Trappist.DomainModel.ApplicationClasses.Test;
using Promact.Trappist.DomainModel.Models.TestConduct;
using System.Collections.Generic;

namespace Promact.Trappist.DomainModel.ApplicationClasses.TestConduct
{
    public class TestBundleModelAC
    {
        public TestAC Test { get; set; }

        public ICollection<TestConductAC> TestQuestions { get; set; }

        public TestAttendees TestAttendee { get; set; }
    }
}