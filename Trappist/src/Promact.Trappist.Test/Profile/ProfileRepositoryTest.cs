using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.Repository.ProfileDetails;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Microsoft.AspNetCore.Identity;
using Promact.Trappist.Web.Models;
using Moq;
using Promact.Trappist.DomainModel.Models;

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

        private IProfileRepository mockRepository()
        {
            var dummyUser = UserDetails();
            var mockProfileRepository = new Mock<IProfileRepository>();
            mockProfileRepository.Setup(x => x.GetUserDetails(dummyUser.Email))
                                 .ReturnsAsync(dummyUser);
            return mockProfileRepository.Object;
        }

        [Fact]
        public async void GetUserDetailsNotNull()
        {
            var testUser = UserDetails();
            IProfileRepository _testUser = mockRepository();
            var userToGet = await _testUser.GetUserDetails(testUser.Email);
            Assert.NotNull(userToGet);
        }

        [Fact]
        public async void GetUserDetailsNull()
        {
            IProfileRepository _testUser = mockRepository();
            var userToGet = await _testUser.GetUserDetails("ritika@gmail.com");
            Assert.Null(userToGet);
        }

        [Fact]
        public void UpdateUserDetails()
        {
            var testUser = UserDetails();
            IProfileRepository _testUser = mockRepository();
            var updatedUser = _testUser.GetUserDetails(testUser.Email);
            if (updatedUser != null)
            {
                updatedUser.Result.Name = "ritika mohata";
                updatedUser.Result.OrganizationName = "Promact InfoTech";
                updatedUser.Result.PhoneNumber = "9876543210";
                _testUser.UpdateProfile(updatedUser.Result);
            };
            Assert.NotSame(updatedUser.Result, testUser);
        }

        [Fact]
        public void ChangeUserPassword()
        {
            var testUserPassword = UserPasswordDetails();
            IProfileRepository _testUser = mockRepository();
            var newUserPassword = new ChangePasswordModel
            {
                Email = "ritika@promactinfo.com",
                OldPassword = "Ritika@123",
                NewPassword = "Ritika_0594"
            };
            var updateUserPassword = _testUser.UpdateUserPassword(newUserPassword);
            Assert.NotSame(updateUserPassword.Result, testUserPassword);
        }

        private ApplicationUser UserDetails()
        {
            var userDetails = new ApplicationUser
            {
                UserName = "ritika@promactinfo.com",
                Email = "ritika@promactinfo.com",
                Name = "Ritika",
                OrganizationName = "Promact",
                PhoneNumber = "9517348625"
            };
            return userDetails;
        }

        private ChangePasswordModel UserPasswordDetails()
        {
            var userPassswordDetails = new ChangePasswordModel
            {
                Email = "ritika@promactinfo.com",
                OldPassword = "Ritika@123"
            };
            return userPassswordDetails;
        }
    }
}
