using Xunit;

namespace Promact.Trappist.Test.Reports
{
    [Collection("Register Dependency")]
    public class ReportsRepositoryTest
    {
        private readonly Bootstrap _bootstrap;

        public ReportsRepositoryTest(Bootstrap bootstrap)
        {
            _bootstrap = bootstrap;
        }
    }
}
