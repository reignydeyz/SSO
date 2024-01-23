using Microsoft.AspNetCore.Identity;
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

        public async Task AddRoles(string username, IEnumerable<ApplicationRole> roles, bool? saveChanges = true)
        {
            var user = await _userManager.FindByEmailAsync(username);

            if (user is null)
                throw new ArgumentNullException();

            await _context.UserRoles.AddRangeAsync(roles.Select(x => new IdentityUserRole<string> { UserId = user.Id, RoleId = x.Id }));

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }

        public Task AddRoles(Guid userId, IEnumerable<ApplicationRole> roles, bool? saveChanges = true)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveRoles(string username, IEnumerable<ApplicationRole> roles, bool? saveChanges = true)
        {
            var user = await _userManager.FindByEmailAsync(username);

            if (user is null)
                throw new ArgumentNullException();

            _context.UserRoles.RemoveRange(roles.Select(x => new IdentityUserRole<string> { UserId = user.Id, RoleId = x.Id }));

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }

        public Task RemoveRoles(Guid userId, IEnumerable<ApplicationRole> roles, bool? saveChanges = true)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApplicationRole>> Roles(Guid userId, Guid applicationId)
        {
            throw new NotImplementedException();
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
