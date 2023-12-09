using System.Security.Claims;

namespace SSO.Domain.Management.Interfaces
{
    public interface IUserClaimRepository
    {
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
