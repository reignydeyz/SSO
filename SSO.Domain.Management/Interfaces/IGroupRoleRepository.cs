using SSO.Domain.Interfaces;
using SSO.Domain.Models;

namespace SSO.Domain.Management.Interfaces
{
    public interface IGroupRoleRepository : IRangeRepository<GroupRole>
    {
        /// <summary>
        /// Gets group`s roles
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        Task<IEnumerable<ApplicationRole>> Roles(Guid groupId, Guid applicationId);

        Task<IEnumerable<GroupRole>> GroupRoles(Guid groupId, Guid applicationId);
    }
}
