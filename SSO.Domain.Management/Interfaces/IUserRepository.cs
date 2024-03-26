using SSO.Domain.Interfaces;
using SSO.Domain.Models;

namespace SSO.Domain.Management.Interfaces
{
    public interface IUserRepository : IRepository<ApplicationUser>, IRangeRepository<ApplicationUser>
    {
        Task<ApplicationUser> GetByUsername(string username);

        Task ChangePassword(ApplicationUser applicationUser, string password, ApplicationUser? author = null);

        Task<IEnumerable<Application>> GetApplications(Guid userId);

        Task<IEnumerable<Group>> GetGroups(Guid userId);
    }
}
