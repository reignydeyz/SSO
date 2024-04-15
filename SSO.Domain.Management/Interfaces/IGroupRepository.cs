using SSO.Domain.Interfaces;
using SSO.Domain.Models;

namespace SSO.Domain.Management.Interfaces
{
    public interface IGroupRepository : IRepository<Group>, IRangeRepository<Group>
    {
        Task<IEnumerable<Application>> GetApplications(Guid groupId);
    }
}
