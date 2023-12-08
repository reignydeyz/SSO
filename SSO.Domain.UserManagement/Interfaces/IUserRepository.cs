using SSO.Domain.Interfaces;
using SSO.Domain.Models;
using System.Security.Claims;

namespace SSO.Domain.UserManegement.Interfaces
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        Task<ApplicationUser> GetByEmail(string email);

        /// <summary>
        /// Gets user claims
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        Task<IEnumerable<Claim>> GetClaims(string userName, Guid applicationId);

        /// <summary>
        /// Gets user claims
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        Task<IEnumerable<Claim>> GetClaims(Guid userId, Guid applicationId);
    }
}
