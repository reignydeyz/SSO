using SSO.Domain.Interfaces;
using SSO.Domain.Models;

namespace SSO.Domain.UserManegement.Interfaces
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        Task<ApplicationUser> GetByEmail(string email);
    }
}
