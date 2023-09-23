using SSO.Domain.Models;

namespace SSO.Domain.Management.Interfaces
{
    public interface IUserRoleManagementService
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
        Task RemoveUserFromRolee(string username, string roleName);

        /// <summary>
        /// Deletes user from role
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task RemoveUserFromRolee(Guid userId, string roleId);

        /// <summary>
        /// Gets user`s roles
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<ApplicationRole>> Roles(Guid userId);

        /// <summary>
        /// Gets user`s roles
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<ApplicationRole>> Roles(string username);
    }
}
