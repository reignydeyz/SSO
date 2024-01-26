using SSO.Domain.Interfaces;
using SSO.Domain.Models;
using System.Security.Claims;

namespace SSO.Domain.Management.Interfaces
{
    public interface IApplicationRoleRepository : IRepository<ApplicationRole>
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

        /// <summary>
        /// Gets users
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<IEnumerable<ApplicationUser>> GetUsers(Guid roleId);
    }
}
