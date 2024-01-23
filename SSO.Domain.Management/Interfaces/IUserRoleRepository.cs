using SSO.Domain.Models;

namespace SSO.Domain.Management.Interfaces
{
    public interface IUserRoleRepository
    {
        /// <summary>
        /// Gets user`s roles
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        Task<IEnumerable<ApplicationRole>> Roles(Guid userId, Guid applicationId);

        /// <summary>
        /// Gets user`s roles
        /// </summary>
        /// <param name="username"></param>
        /// <param name="applicationId"
        /// <returns></returns>
        Task<IEnumerable<ApplicationRole>> Roles(string username, Guid applicationId);
    }
}
