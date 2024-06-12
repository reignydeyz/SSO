using Microsoft.AspNetCore.Identity;
using SSO.Domain.Interfaces;
using SSO.Domain.Models;
using SSO.Infrastructure.Management;
using System.Linq.Expressions;

namespace SSO.Infrastructure.LDAP
{
    public class UserRepository : UserRepositoryBase
    {
        readonly IAppDbContext _context;

        public UserRepository(UserManager<ApplicationUser> userManager, IAppDbContext context) : base(userManager, context)
        {
            _context = context;
        }

        public override async Task<ApplicationUser> Add(ApplicationUser param, bool? saveChanges = true, object? args = null)
        {
            _context.Add(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();

            return param;
        }

        public override async Task AddRange(IEnumerable<ApplicationUser> param, bool? saveChanges = true, object? args = null)
        {
            _context.AddRange(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }

        public override Task ChangePassword(ApplicationUser applicationUser, string password, ApplicationUser? author = null)
        {
            throw new NotImplementedException();
        }

        public override Task Delete(ApplicationUser param, bool? saveChanges = true)
        {
            throw new NotImplementedException();
        }

        public override async Task RemoveRange(IEnumerable<ApplicationUser> param, bool? saveChanges = true, object? args = null)
        {
            _context.RemoveRange(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }

        public override async Task RemoveRange(Expression<Func<ApplicationUser, bool>> predicate, bool? saveChanges = true, object? args = null)
        {
            _context.RemoveRange(_context.ApplicationUsers.Where(predicate));

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }

        public override Task<ApplicationUser> Update(ApplicationUser param, bool? saveChanges = true, object? args = null)
        {
            throw new NotImplementedException();
        }
    }
}
