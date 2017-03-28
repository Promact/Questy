using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.Repository.ProfileDetails;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Microsoft.AspNetCore.Identity;
using Promact.Trappist.Web.Models;

namespace Promact.Trappist.Test.Profile
{
    [Collection("Register Dependency")]
    public class ProfileRepositoryTest
    {
        private readonly Bootstrap _bootstrap;
        private readonly TrappistDbContext _dbContext;
        private readonly IProfileRepository _profileRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileRepositoryTest(Bootstrap bootstrap)
        {
            _bootstrap = bootstrap;
            _dbContext = bootstrap.ServiceProvider.GetService<TrappistDbContext>();
            _profileRepository = bootstrap.ServiceProvider.GetService<IProfileRepository>();
            _userManager = bootstrap.ServiceProvider.GetService<UserManager<ApplicationUser>>();
            ClearDatabase.ClearDatabaseAndSeed(_dbContext);
        }
    }
}
