using Microsoft.AspNetCore.Identity;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;
using System.Security.Claims;

namespace SSO.Infrastructure.Management
{
    public class RoleRepository : IRoleRepository
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RoleRepository(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public Task<ApplicationRole> Add(ApplicationRole param)
        {
            throw new NotImplementedException();
        }

        public Task Delete(ApplicationRole param)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApplicationRole>> Find(Func<ApplicationRole, bool>? predicate)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationRole> FindOne(Func<ApplicationRole, bool> predicate)
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

        public Task<ApplicationRole> Update(ApplicationRole param)
        {
            throw new NotImplementedException();
        }
    }
}
