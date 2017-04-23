using Xunit;

namespace Promact.Trappist.Test.Tests
{
    [Collection("Register Dependency")]
    public class TestsRepositoryTest
    {
        private readonly Bootstrap _bootstrap;

        public TestsRepositoryTest(Bootstrap bootstrap)
        {
            _bootstrap = bootstrap;
        }
    }
}
