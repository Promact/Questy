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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IProfileRepository _profileRepository;

        public ProfileRepositoryTest(Bootstrap bootstrap) : base(bootstrap)
        {
            //resolve dependency to be used in tests
            _profileRepository = _scope.ServiceProvider.GetService<IProfileRepository>();
            _userManager = _scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
        }

        /// <summary>
        /// Test case to get the details of the user
        /// </summary>
        [Fact]
        public async Task GetUserDetailsAsync()
        {
            var user = UserDetails();
            await _userManager.CreateAsync(user);
            var result = await _profileRepository.GetUserDetailsAsync(user.Email);
            Assert.Equal(result, user);
        }

        /// <summary>
        /// Test case to update profile details of the user
        /// </summary>
        [Fact]
        public async Task UpdateProfile()
        {
            var user = UserDetails();
            await _userManager.CreateAsync(user);
            var userToUpdate = await _profileRepository.GetUserDetailsAsync(user.Email);
            userToUpdate.Name = "Ritika Mohata";
            userToUpdate.OrganizationName = "Promact Infotect";
            userToUpdate.PhoneNumber = "7003957635";
            _profileRepository.UpdateUserProfile(userToUpdate);
            Assert.True(_trappistDbContext.Users.Count(x => x.Name == userToUpdate.Name) == 1);
            Assert.True(_trappistDbContext.Users.Count(x => x.OrganizationName == userToUpdate.OrganizationName) == 1);
            Assert.True(_trappistDbContext.Users.Count(x => x.PhoneNumber == userToUpdate.PhoneNumber) == 1);
        }

        /// <summary>
        /// Test case to update new password of the user
        /// </summary>
        /// <returns>Returns true and updates the new password of the user</returns>
        [Fact]
        public async Task UpdateUserPasswordAsync()
        {
            var user = UserDetails();
            var passwordOfUser = Password();
            await _userManager.CreateAsync(user, passwordOfUser.OldPassword);
            var result = await _profileRepository.UpdateUserPasswordAsync(user.Email, passwordOfUser);
            Assert.True(result);
        }

        /// <summary>
        /// test case to check invalid old password
        /// </summary>
        /// <returns>returns false and does not update the new password as old password is wrong</returns>
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
