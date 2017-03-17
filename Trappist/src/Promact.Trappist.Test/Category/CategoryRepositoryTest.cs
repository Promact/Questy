using Xunit;

namespace Promact.Trappist.Test.Category
{
    [Collection("Register Dependency")]
    public class CategoryRepositoryTest
    {
        private readonly Bootstrap _bootstrap;

        public CategoryRepositoryTest(Bootstrap bootstrap)
        {
            _bootstrap = bootstrap;
        }
    }
}
