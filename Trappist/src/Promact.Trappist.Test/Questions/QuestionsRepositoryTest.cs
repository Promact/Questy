using Microsoft.Extensions.DependencyInjection;
using Promact.Trappist.DomainModel.DbContext;
using Xunit;

namespace Promact.Trappist.Test.Questions
{
    [Collection("Register Dependency")]
    public class QuestionsRepositoryTest
    {
        private readonly Bootstrap _bootstrap;
        private readonly TrappistDbContext _trappistDbContext;

        public QuestionsRepositoryTest(Bootstrap bootstrap)
        {
            _bootstrap = bootstrap;
            //resolve dependency to be used in tests
            _trappistDbContext = _bootstrap.ServiceProvider.GetService<TrappistDbContext>();
            ClearDatabase.ClearDatabaseAndSeed(_trappistDbContext);
        }
               
    }
}
