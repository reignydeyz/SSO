using SSO.Domain.Interfaces;
using SSO.Domain.Models;
using System.Security.Claims;

namespace SSO.Domain.Management.Interfaces
{
    public interface IRoleRepository : IRepository<ApplicationRole>
    {
        /// <summary>
        /// Gets role claims
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        Task<IEnumerable<Claim>> GetClaims(string roleName);

        /// <summary>
        /// Gets role claims
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<IEnumerable<Claim>> GetClaims(Guid roleId);
    }
}
