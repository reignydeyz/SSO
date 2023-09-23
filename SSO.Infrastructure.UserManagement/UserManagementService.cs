using Microsoft.AspNetCore.Identity;
using SSO.Domain.Models;
using SSO.Domain.UserManegement.Interfaces;

namespace SSO.Infrastructure.UserManagement
{
    public class UserManagementService : IUserManagementService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserManagementService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public Task<ApplicationUser> Add(ApplicationUser param)
        {
            throw new NotImplementedException();
        }

        public Task Delete(ApplicationUser param)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApplicationUser>> Find(Func<ApplicationUser, bool>? predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<ApplicationUser> GetByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
                throw new ArgumentNullException();

            return user;
        }

        public Task<ApplicationUser> FindOne(Func<ApplicationUser, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> Update(ApplicationUser param)
        {
            throw new NotImplementedException();
        }
    }
}
