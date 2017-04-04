using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.Repository.Profile;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Microsoft.AspNetCore.Identity;
using Promact.Trappist.Web.Models;

namespace Promact.Trappist.Test.Profile
{
    [Collection("Register Dependency")]
    public class ProfileRepositoryTest : BaseTest
    {
        public ProfileRepositoryTest(Bootstrap bootstrap) : base(bootstrap)
        {

        }
    }
}
