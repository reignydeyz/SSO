using SSO.Domain.Interfaces;
using SSO.Domain.Models;

namespace SSO.Domain.Management.Interfaces
{
    public interface IApplicationRepository : IRepository<Application>
    {
        public Task<IEnumerable<string>> GetPermissions(Guid applicationId);

        public Task<IEnumerable<string>> GetCallbackUrls(Guid applicationId);

        public Task<IEnumerable<Application>> GetAppsByUserId(Guid userId);
    }
}
