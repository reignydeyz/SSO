using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SSO.Domain.Interfaces;
using SSO.Domain.Models;

namespace SSO.Infrastructure.Management
{
    public class UserRoleManagementService : IUserRoleManagementService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public UserRoleManagementService(UserManager<ApplicationUser> userManager, AppDbContext context)
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

        public Task RemoveUserFromRolee(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task RemoveUserFromRolee(Guid userId, string roleId)
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
