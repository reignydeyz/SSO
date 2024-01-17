using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;
using System.Linq.Expressions;
using System.Security.Claims;

namespace SSO.Infrastructure.Management
{
    public class ApplicationRoleRepository : IApplicationRoleRepository
    {
        readonly RoleManager<ApplicationRole> _roleManager;
        readonly AppDbContext _context;

        public ApplicationRoleRepository(RoleManager<ApplicationRole> roleManager, AppDbContext context)
        {
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<ApplicationRole> Add(ApplicationRole param, object? args)
        {
            param.Id = Guid.NewGuid().ToString();
            param.NormalizedName = param.Name!.ToUpper();

            _context.Roles.Add(param);

            await _context.SaveChangesAsync();

            return param;
        }

        public async Task<bool> Any(Expression<Func<ApplicationRole, bool>> predicate)
        {
            return await _context.ApplicationRoles.AnyAsync(predicate);
        }

        public Task Delete(ApplicationRole param)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<ApplicationRole>> Find(Expression<Func<ApplicationRole, bool>>? predicate)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationRole> FindOne(Expression<Func<ApplicationRole, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Claim>> GetClaims(ApplicationRole role)
        {
            throw new NotImplementedException();
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

        public Task<ApplicationRole> Update(ApplicationRole param, object? args)
        {
            throw new NotImplementedException();
        }
    }
}
