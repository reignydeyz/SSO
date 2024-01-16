using Microsoft.AspNetCore.Identity;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;
using System.Linq.Expressions;
using System.Security.Claims;

namespace SSO.Infrastructure.Management
{
    public class ApplicationRoleRepository : IApplicationRoleRepository
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public ApplicationRoleRepository(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<ApplicationRole> Add(ApplicationRole param, object? args)
        {
            param.Id = Guid.NewGuid().ToString();

            var res = await _roleManager.CreateAsync(param);

            if (res.Succeeded)
                return await _roleManager.FindByNameAsync(param.Name);
            else
                throw new ArgumentException(res.Errors.First().Description);
        }

        public Task<bool> Any(Expression<Func<ApplicationRole, bool>> predicate)
        {
            throw new NotImplementedException();
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
