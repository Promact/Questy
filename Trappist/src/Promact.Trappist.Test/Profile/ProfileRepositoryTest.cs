using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.Repository.Profile;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Microsoft.AspNetCore.Identity;
using Promact.Trappist.Web.Models;
using System.Threading.Tasks;
using System.Linq;
using Promact.Trappist.DomainModel.Models;

namespace Promact.Trappist.Test.Profile
{
    [Collection("Register Dependency")]
    public class ProfileRepositoryTest : BaseTest
    {
        public ProfileRepositoryTest(Bootstrap bootstrap) : base(bootstrap)
        {

        }
    public class ProfileRepositoryTest
    {
        private readonly Bootstrap _bootstrap;
        private readonly TrappistDbContext _trappistDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IProfileRepository _profileRepository;

        public ProfileRepositoryTest(Bootstrap bootstrap)
        {
            _bootstrap = bootstrap;
            //resolve dependency to be used in tests
            _trappistDbContext = _bootstrap.ServiceProvider.GetService<TrappistDbContext>();
            _profileRepository = _bootstrap.ServiceProvider.GetService<IProfileRepository>();
            _userManager = _bootstrap.ServiceProvider.GetService<UserManager<ApplicationUser>>();
            ClearDatabase.ClearDatabaseAndSeed(_trappistDbContext);
        }

        [Fact]
        public async Task GetUserDetailsAsync()
        {
            var user = UserDetails();
            await _userManager.CreateAsync(user);
            var result = await _profileRepository.GetUserDetailsAsync(user.Email);
            Assert.Equal(result, user);
        }

        [Fact]
        public async void UpdateProfile()
        {
            var user = UserDetails();
            await _userManager.CreateAsync(user);
            var userToUpdate = new ApplicationUser();
            userToUpdate = await _profileRepository.GetUserDetailsAsync(user.Email);
            userToUpdate.Name = "Ritika Mohata";
            userToUpdate.OrganizationName = "Promact Infotect";
            userToUpdate.PhoneNumber = "7003957635";
            _profileRepository.UpdateUserProfile(userToUpdate);
            Assert.True(_trappistDbContext.Users.Count(x => x.Name == userToUpdate.Name) == 1);
            Assert.True(_trappistDbContext.Users.Count(x => x.OrganizationName == userToUpdate.OrganizationName) == 1);
            Assert.True(_trappistDbContext.Users.Count(x => x.PhoneNumber == userToUpdate.PhoneNumber) == 1);
        }

        [Fact]
        public async Task UpdateUserPasswordAsync()
        {
            var user = UserDetails();
            var passwordOfUser = Password();
            await _userManager.CreateAsync(user, passwordOfUser.OldPassword);
            var result = await _profileRepository.UpdateUserPasswordAsync(user.Email, passwordOfUser);
            Assert.True(result);
        }

        [Fact]
        public async Task InvalidUpdateUserPasswordAsync()
        {
            var user = UserDetails();
            var passwordOfUser = Password();
            await _userManager.CreateAsync(user, passwordOfUser.NewPassword);
            var result = await _profileRepository.UpdateUserPasswordAsync(user.Email, passwordOfUser);
            Assert.False(result);
        }

        private ApplicationUser UserDetails()
        {
            var userDetails = new ApplicationUser()
            {
                Name = "Ritika",
                OrganizationName = "Promact",
                Email = "ritika@promactinfo.com",
                UserName = "ritika@promactinfo.com",
                PhoneNumber = "9804591097"
            };
            return userDetails;
        }

        private ChangePasswordModel Password()
        {
            var userPassword = new ChangePasswordModel()
            {
                OldPassword = "RItika@123",
                NewPassword = "Ritika_05",
                ConfirmPassword = "Ritika_05"
            };
            return userPassword;
        }
    }
}
