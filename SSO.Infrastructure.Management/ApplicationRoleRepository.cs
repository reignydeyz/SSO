using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SSO.Domain.Interfaces;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;
using System.Linq.Expressions;
using System.Security.Claims;

namespace SSO.Infrastructure.Management
{
    public class ApplicationRoleRepository : IApplicationRoleRepository
    {
        readonly RoleManager<ApplicationRole> _roleManager;
        readonly IAppDbContext _context;

        public ApplicationRoleRepository(RoleManager<ApplicationRole> roleManager, IAppDbContext context)
        {
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<ApplicationRole> Add(ApplicationRole param, bool? saveChanges = true, object? args = null)
        {
            param.Id = Guid.NewGuid().ToString();
            param.NormalizedName = param.Name!.ToUpper();

            _context.Roles.Add(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();

            return param;
        }

        public async Task<bool> Any(Expression<Func<ApplicationRole, bool>> predicate)
        {
            return await _context.ApplicationRoles.AnyAsync(predicate);
        }

        public async Task Delete(ApplicationRole param, bool? saveChanges = true, object? args = null)
        {
            _context.Remove(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }

        public async Task<IQueryable<ApplicationRole>> Find(Expression<Func<ApplicationRole, bool>>? predicate)
        {
            if (predicate is not null)
                return await Task.Run(() => _context.ApplicationRoles.Where(predicate).AsQueryable().AsNoTracking());
            else
                return _context.ApplicationRoles.AsQueryable().AsNoTracking();
        }

        public async Task<ApplicationRole> FindOne(Expression<Func<ApplicationRole, bool>> predicate)
        {
            return await _context.ApplicationRoles.FirstOrDefaultAsync(predicate);
        }

        public Task<IEnumerable<Claim>> GetClaims(string roleName)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Claim>> GetClaims(Guid roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());

            return await _roleManager.GetClaimsAsync(role);
        }

        public Task<ApplicationRole> Update(ApplicationRole param, bool? saveChanges = true, object? args = null)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsers(Guid roleId)
        {
            var users = from r in _context.Roles.Where(x => x.Id == roleId.ToString())
                        join ur in _context.UserRoles on r.Id equals ur.RoleId
                        join u in _context.Users on ur.UserId equals u.Id
                        select u;

            return await users.Distinct().ToListAsync();
        }

        public async Task AddRange(IEnumerable<ApplicationRole> param, bool? saveChanges = true, object? args = null)
        {
            _context.AddRange(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }

        public Task RemoveRange(IEnumerable<ApplicationRole> param, bool? saveChanges = true, object? args = null)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ApplicationPermission>> GetPermissions(Guid roleId)
        {
            var idStr = roleId.ToString();
            var res = _context.RoleClaims.Where(x => x.RoleId == idStr).Select(x => x.Permission);

            return res.ToList();
        }

        public Task<IEnumerable<ApplicationPermission>> GetPermissions(string roleName)
        {
            throw new NotImplementedException();
        }

        public Task RemoveRange(Expression<Func<ApplicationRole, bool>> predicate, bool? saveChanges = true, object? args = null)
        {
            throw new NotImplementedException();
        }
    }
}
