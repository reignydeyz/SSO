using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;

namespace SSO.Infrastructure.Management
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public UserRoleRepository(UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public Task AssignUserToRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task AssignUserToRole(Guid userId, string roleId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveUserFromRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task RemoveUserFromRole(Guid userId, string roleId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApplicationRole>> Roles(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ApplicationRole>> Roles(string username)
        {
            var user = await _userManager.FindByEmailAsync(username);

            if (user is null)
                throw new ArgumentNullException();

            var roles = await _userManager.GetRolesAsync(user);

            return _context.Roles.Include(x => x.Application).Where(x => roles.Contains(x.Name));
        }
    }
}
