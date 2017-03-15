using Xunit;

namespace Promact.Trappist.Test.BasicSetup
{
    [Collection("Register Dependency")]
    public class BasicSetupRepositoryTest
    {
        private readonly Bootstrap _bootstrap;

        public BasicSetupRepositoryTest(Bootstrap bootstrap)
        {
            _bootstrap = bootstrap;
        }
    }
}
