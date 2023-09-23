using SSO.Domain.Models;

namespace SSO.Domain.Interfaces
{
    public interface IUserManagementService : IRepository<ApplicationUser>
    {
        Task<ApplicationUser> GetByEmail(string email);
    }
}
