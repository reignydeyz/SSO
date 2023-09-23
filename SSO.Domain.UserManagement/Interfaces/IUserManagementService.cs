using SSO.Domain.Interfaces;
using SSO.Domain.Models;

namespace SSO.Domain.UserManegement.Interfaces
{
    public interface IUserManagementService : IRepository<ApplicationUser>
    {
        Task<ApplicationUser> GetByEmail(string email);
    }
}
