using Microsoft.AspNetCore.Identity;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;
using System.Data;

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

        public async Task AddRoles(string username, IEnumerable<ApplicationRole> roles, bool? saveChanges = true)
        {
            var user = await _userManager.FindByEmailAsync(username);

            if (user is null)
                throw new ArgumentNullException();

            var toBeAdded = roles.Select(x => new IdentityUserRole<string> { UserId = user.Id, RoleId = x.Id }).ToList();

            await _context.UserRoles.AddRangeAsync(toBeAdded);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }

        public async Task AddRoles(Guid userId, IEnumerable<ApplicationRole> roles, bool? saveChanges = true)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            await AddRoles(user.UserName, roles, saveChanges);
        }

        public async Task RemoveRoles(string username, IEnumerable<ApplicationRole> roles, bool? saveChanges = true)
        {
            var user = await _userManager.FindByEmailAsync(username);

            if (user is null)
                throw new ArgumentNullException();

            var toBeDeleted = _context.UserRoles.Where(x => x.UserId == user.Id && roles.Select(x => x.Id).Contains(x.RoleId.ToString()));

            _context.UserRoles.RemoveRange(toBeDeleted);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }

        public async Task RemoveRoles(Guid userId, IEnumerable<ApplicationRole> roles, bool? saveChanges = true)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            await RemoveRoles(user.UserName, roles, saveChanges);
        }

        public async Task<IEnumerable<ApplicationRole>> Roles(Guid userId, Guid applicationId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            return await Roles(user.UserName, applicationId);
        }

        public async Task<IEnumerable<ApplicationRole>> Roles(string username, Guid applicationId)
        {
            var user = await _userManager.FindByEmailAsync(username);

            if (user is null)
                throw new ArgumentNullException();

            var roles = from r in _context.Roles.Where(x => x.ApplicationId == applicationId)
                      join ur in _context.UserRoles.Where(x => x.UserId == user.Id) on r.Id equals ur.RoleId
                      select r;

            return roles;
        }
    }
}
