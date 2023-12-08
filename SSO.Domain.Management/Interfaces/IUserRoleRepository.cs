using SSO.Domain.Models;

namespace SSO.Domain.Management.Interfaces
{
    public interface IUserRoleRepository
    {
        /// <summary>
        /// Adds user to role
        /// </summary>
        /// <param name="username"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        Task AssignUserToRole(string username, string roleName);

        /// <summary>
        /// Adds user to role
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task AssignUserToRole(Guid userId, string roleId);

        /// <summary>
        /// Deletes user from role
        /// </summary>
        /// <param name="username"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        Task RemoveUserFromRole(string username, string roleName);

        /// <summary>
        /// Deletes user from role
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task RemoveUserFromRole(Guid userId, string roleId);

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
