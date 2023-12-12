using SSO.Domain.Interfaces;
using SSO.Domain.Models;

namespace SSO.Domain.UserManagement.Interfaces
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        Task<ApplicationUser> GetByEmail(string email);
    }
}
