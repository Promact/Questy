using Microsoft.Extensions.DependencyInjection;
using Promact.Trappist.DomainModel.DbContext;
using Xunit;

namespace Promact.Trappist.Test.Questions
{
    [Collection("Register Dependency")]
    public class QuestionsRepositoryTest : BaseTest
    {
        public QuestionsRepositoryTest(Bootstrap bootstrap) : base(bootstrap)
        {   

        }
               
    }
}
