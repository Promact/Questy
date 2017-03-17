using Xunit;

namespace Promact.Trappist.Test.TestConduct
{
    [Collection("Register Dependency")]
    public class TestConductRepositoryTest
    {
        private readonly Bootstrap _bootstrap;

        public TestConductRepositoryTest(Bootstrap bootstrap)
        {
            _bootstrap = bootstrap;
        }
    }
}
