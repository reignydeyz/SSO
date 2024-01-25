using SSO.Domain.Interfaces;
using SSO.Domain.Models;

namespace SSO.Domain.Management.Interfaces
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        Task<ApplicationUser> GetByEmail(string email);

        Task ChangePassword(ApplicationUser applicationUser, string password, ApplicationUser? author = null);

        Task<IEnumerable<Application>> GetApplications(Guid userId);
    }
}
