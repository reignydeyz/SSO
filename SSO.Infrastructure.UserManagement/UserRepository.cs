using Microsoft.AspNetCore.Identity;
using SSO.Domain.Models;
using SSO.Domain.UserManegement.Interfaces;
using System.Linq.Expressions;

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

        public Task<IEnumerable<ApplicationUser>> Find(Expression<Func<ApplicationUser, bool>>? predicate)
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

        public Task<ApplicationUser> FindOne(Expression<Func<ApplicationUser, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> Update(ApplicationUser param)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Any(Expression<Func<ApplicationUser, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
