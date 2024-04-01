using SSO.Domain.Interfaces;
using SSO.Domain.Models;

namespace SSO.Domain.Management.Interfaces
{
    public interface IApplicationRepository : IRepository<Application>
    {
        public Task<IEnumerable<ApplicationPermission>> GetPermissions(Guid applicationId);

        public Task<IEnumerable<ApplicationCallback>> GetCallbacks(Guid applicationId);

        public Task<IEnumerable<ApplicationRole>> GetRoles(Guid applicationId);

        public Task<IEnumerable<Application>> GetAppsByUserId(Guid userId);

        public Task<IQueryable<ApplicationUser>> GetUsers(Guid applicationId);

        public Task<IQueryable<Group>> GetGroups(Guid applicationId);
    }
}
