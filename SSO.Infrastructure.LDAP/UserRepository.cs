using Microsoft.AspNetCore.Identity;
using SSO.Domain.Models;
using SSO.Infrastructure.Management;

namespace SSO.Infrastructure.LDAP
{
    public class UserRepository : UserRepositoryBase
    {
        public UserRepository(UserManager<ApplicationUser> userManager, AppDbContext context) : base(userManager, context)
        {
        }

        public override Task<ApplicationUser> Add(ApplicationUser param, bool? saveChanges = true, object? args = null)
        {
            throw new NotImplementedException();
        }

        public override Task ChangePassword(ApplicationUser applicationUser, string password, ApplicationUser? author = null)
        {
            throw new NotImplementedException();
        }

        public override Task Delete(ApplicationUser param, bool? saveChanges = true)
        {
            throw new NotImplementedException();
        }

        public override Task<ApplicationUser> Update(ApplicationUser param, bool? saveChanges = true, object? args = null)
        {
            throw new NotImplementedException();
        }
    }
}
