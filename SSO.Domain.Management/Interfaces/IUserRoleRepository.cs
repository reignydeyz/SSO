using SSO.Domain.Models;

namespace SSO.Domain.Management.Interfaces
{
    public interface IUserRoleRepository
    {
        /// <summary>
        /// Adds roles to user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="roles"></param>
        /// <param name="saveChanges"></param>
        /// <returns></returns>
        Task AddRoles(string username, IEnumerable<ApplicationRole> roles, bool? saveChanges = true);

        /// <summary>
        /// Adds roles to user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roles"></param>
        /// <param name="saveChanges"></param>
        /// <returns></returns>
        Task AddRoles(Guid userId, IEnumerable<ApplicationRole> roles, bool? saveChanges = true);

        /// <summary>
        /// Deletes roles from user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="roles"></param>
        /// <param name="saveChanges"></param>
        /// <returns></returns>
        Task RemoveRoles(string username, IEnumerable<ApplicationRole> roles, bool? saveChanges = true);

        /// <summary>
        /// Deletes roles from user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roles"></param>
        /// <param name="saveChanges"></param>
        /// <returns></returns>
        Task RemoveRoles(Guid userId, IEnumerable<ApplicationRole> roles, bool? saveChanges = true);

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

        Task RemoveUser(Guid userId, Guid applicationId, bool? saveChanges = true);

        Task RemoveUser(string username, Guid applicationId, bool? saveChanges = true);

        Task Clear(Guid userId, bool? saveChanges = true);

        Task Clear(string username, bool? saveChanges = true);
    }
}
