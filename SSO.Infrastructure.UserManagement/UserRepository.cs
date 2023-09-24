using Microsoft.AspNetCore.Identity;
using SSO.Domain.Models;
using SSO.Domain.UserManegement.Interfaces;
using System.Security.Claims;

namespace SSO.Infrastructure.UserManagement
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(UserManager<ApplicationUser> userManager)
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

        public Task<IEnumerable<Claim>> GetClaims(string userName)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Claim>> GetClaims(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            return await _userManager.GetClaimsAsync(user);
        }
    }
}
