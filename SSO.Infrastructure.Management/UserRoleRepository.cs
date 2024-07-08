using Microsoft.AspNetCore.Identity;
using SSO.Domain.Interfaces;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;
using System.Data;

namespace SSO.Infrastructure.Management
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAppDbContext _context;

        public UserRoleRepository(UserManager<ApplicationUser> userManager, IAppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task AddRoles(string username, IEnumerable<ApplicationRole> roles, bool? saveChanges = true)
        {
            var user = await _userManager.FindByNameAsync(username) ?? throw new ArgumentNullException();

            await AddRoles(user, roles, saveChanges);
        }

        public async Task AddRoles(Guid userId, IEnumerable<ApplicationRole> roles, bool? saveChanges = true)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString()) ?? throw new ArgumentNullException();

            await AddRoles(user, roles, saveChanges);
        }

        public async Task RemoveRoles(string username, IEnumerable<ApplicationRole> roles, bool? saveChanges = true)
        {
            var user = await _userManager.FindByNameAsync(username) ?? throw new ArgumentNullException();

            await RemoveRoles(user, roles, saveChanges);
        }

        public async Task RemoveRoles(Guid userId, IEnumerable<ApplicationRole> roles, bool? saveChanges = true)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString()) ?? throw new ArgumentNullException();

            await RemoveRoles(user, roles, saveChanges);
        }

        public async Task RemoveUser(Guid userId, Guid applicationId, bool? saveChanges = true)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString()) ?? throw new ArgumentNullException();

            await RemoveUser(user, applicationId, saveChanges);
        }

        public async Task RemoveUser(string username, Guid applicationId, bool? saveChanges = true)
        {
            var user = await _userManager.FindByNameAsync(username) ?? throw new ArgumentNullException();

            await RemoveUser(user, applicationId, saveChanges);
        }

        public async Task<IEnumerable<ApplicationRole>> Roles(Guid userId, Guid applicationId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString()) ?? throw new ArgumentNullException();

            return await Roles(user, applicationId);
        }

        public async Task<IEnumerable<ApplicationRole>> Roles(string username, Guid applicationId)
        {
            var user = await _userManager.FindByNameAsync(username) ?? throw new ArgumentNullException();

            return await Roles(user, applicationId);
        }

        private async Task AddRoles(ApplicationUser user, IEnumerable<ApplicationRole> roles, bool? saveChanges = true)
        {
            var toBeAdded = roles.Select(x => new IdentityUserRole<string> { UserId = user.Id, RoleId = x.Id }).ToList();

            await _context.UserRoles.AddRangeAsync(toBeAdded);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }

        private async Task RemoveRoles(ApplicationUser user, IEnumerable<ApplicationRole> roles, bool? saveChanges = true)
        {
            if (roles.Any())
            {
                var roleIds = roles.Select(r => r.Id.ToString()).ToList();
                var roleIdsString = roleIds.Select(id => id.ToString()).ToList(); // Convert roleIds to string again, not using ToString().

                var toBeDeleted = _context.UserRoles
                    .Where(x => x.UserId == user.Id && roleIdsString.Contains(x.RoleId)) // Use roleIdsString directly, which is already a list of strings.
                    .ToList();

                _context.UserRoles.RemoveRange(toBeDeleted);

                if (saveChanges!.Value)
                    await _context.SaveChangesAsync();
            }
        }

        private async Task RemoveUser(ApplicationUser user, Guid applicationId, bool? saveChanges = true)
        {
            var toBeDeleted = _context.UserRoles.Where(x => x.UserId == user.Id && _context.Roles.Any(y => y.ApplicationId == applicationId && y.Id == x.RoleId));

            _context.UserRoles.RemoveRange(toBeDeleted);

            await _context.SaveChangesAsync();
        }

        private async Task<IEnumerable<ApplicationRole>> Roles(ApplicationUser user, Guid applicationId)
        {
            var roles = from r in _context.Roles.Where(x => x.ApplicationId == applicationId)
                        join ur in _context.UserRoles.Where(x => x.UserId == user.Id) on r.Id equals ur.RoleId
                        select r;

            return roles;
        }
    }
}
