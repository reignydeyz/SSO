using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SSO.Domain.Interfaces;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;
using System.Linq.Expressions;

namespace SSO.Infrastructure.Management
{
    public abstract class UserRepositoryBase : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAppDbContext _context;

        protected UserRepositoryBase(UserManager<ApplicationUser> userManager, IAppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public abstract Task<ApplicationUser> Add(ApplicationUser param, bool? saveChanges = true, object? args = null);

        public abstract Task AddRange(IEnumerable<ApplicationUser> param, bool? saveChanges = true, object? args = null);

        public async Task<bool> Any(Expression<Func<ApplicationUser, bool>> predicate)
        {
            return await _context.ApplicationUsers.AnyAsync(predicate);
        }

        public abstract Task ChangePassword(ApplicationUser applicationUser, string password, ApplicationUser? author = null, object? args = null);

        public abstract Task Delete(ApplicationUser param, bool? saveChanges = true, object? args = null);

        public async Task<IQueryable<ApplicationUser>> Find(Expression<Func<ApplicationUser, bool>>? predicate)
        {
            if (predicate is not null)
                return await Task.Run(() => _context.ApplicationUsers.Where(predicate).AsQueryable().AsNoTracking());
            else
                return _context.ApplicationUsers.AsQueryable().AsNoTracking();
        }

        public async Task<ApplicationUser> FindOne(Expression<Func<ApplicationUser, bool>> predicate)
        {
            return await _context.ApplicationUsers.FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<Application>> GetApplications(Guid userId)
        {
            var apps = from ur in _context.UserRoles.Where(x => x.UserId == userId.ToString())
                       join r in _context.Roles on ur.RoleId equals r.Id
                       join a in _context.Applications on r.ApplicationId equals a.ApplicationId
                       select a;

            return await apps.Distinct().ToListAsync();
        }

        public async Task<ApplicationUser> GetByUsername(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user is null)
                throw new ArgumentNullException();

            return user;
        }

        public async Task<IEnumerable<Group>> GetGroups(Guid userId)
        {
            var groups = _context.GroupUsers
                            .Include(x => x.Group)
                            .Where(x => x.UserId == userId.ToString())
                            .Select(x => x.Group);

            return await groups.Distinct().ToListAsync();
        }

        public async Task<IEnumerable<Realm>> GetRealms(Guid userId)
        {
            var user = await _context.Users
                .Include(x => x.Realms).ThenInclude(x => x.Realm)
                .FirstAsync(x => x.Id == userId.ToString());

            return user.Realms.Select(x => x.Realm);
        }

        public abstract Task RemoveRange(IEnumerable<ApplicationUser> param, bool? saveChanges = true, object? args = null);

        public abstract Task RemoveRange(Expression<Func<ApplicationUser, bool>> predicate, bool? saveChanges = true, object? args = null);

        public abstract Task<ApplicationUser> Update(ApplicationUser param, bool? saveChanges = true, object? args = null);
    }
}
