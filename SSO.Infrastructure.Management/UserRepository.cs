using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;
using System.Linq.Expressions;

namespace SSO.Infrastructure.Management
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public UserRepository(UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<ApplicationUser> Add(ApplicationUser param, bool? saveChanges = true, object? args = null)
        {
            var password = param.PasswordHash ?? Guid.NewGuid().ToString();
            param.PasswordHash = null;

            var res = await _userManager.CreateAsync(param, password);

            if (!res.Succeeded && res.Errors.Any())
                throw new ArgumentException(res.Errors.First().Description);

            return await _userManager.FindByNameAsync(param.UserName);
        }

        public async Task Delete(ApplicationUser param, bool? saveChanges = true)
        {
            await _userManager.DeleteAsync(param);
        }

        public async Task<IQueryable<ApplicationUser>> Find(Expression<Func<ApplicationUser, bool>>? predicate)
        {
            if (predicate is not null)
                return await Task.Run(() => _context.ApplicationUsers.Where(predicate).AsQueryable().AsNoTracking());
            else
                return _context.ApplicationUsers.AsQueryable().AsNoTracking();
        }

        public async Task<ApplicationUser> GetByUsername(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user is null)
                throw new ArgumentNullException();

            return user;
        }

        public async Task<ApplicationUser> FindOne(Expression<Func<ApplicationUser, bool>> predicate)
        {
            return await _context.ApplicationUsers.FirstOrDefaultAsync(predicate);
        }

        public async Task<ApplicationUser> Update(ApplicationUser param, bool? saveChanges = true, object? args = null)
        {
            var rec = _context.ApplicationUsers.First(x => x.Id == param.Id);
            rec.FirstName = param.FirstName;
            rec.LastName = param.LastName;
            rec.UserName = param.UserName;
            rec.NormalizedUserName = param.UserName.ToUpper();
            rec.Email = param.Email;

            await _context.SaveChangesAsync();

            if (param.PasswordHash is not null)
                await ChangePassword(rec, param.PasswordHash, default);

            return await _userManager.FindByNameAsync(param.UserName);
        }

        public async Task<bool> Any(Expression<Func<ApplicationUser, bool>> predicate)
        {
            return await _context.ApplicationUsers.AnyAsync(predicate);
        }

        public async Task ChangePassword(ApplicationUser applicationUser, string password, ApplicationUser? author)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(applicationUser);

            var res = await _userManager.ResetPasswordAsync(applicationUser, token, password);

            if (!res.Succeeded && res.Errors.Any())
                throw new ArgumentException(res.Errors.First().Description);

            var rec = _context.ApplicationUsers.First(x => x.Id == applicationUser.Id);

            rec.DateModified = DateTime.Now;
            rec.ModifiedBy = author == null ? $"{applicationUser.FirstName} {applicationUser.LastName}" : $"{author.FirstName} {author.LastName}";

            _context.SaveChanges();
        }

        public async Task<IEnumerable<Application>> GetApplications(Guid userId)
        {
            var apps = from ur in _context.UserRoles.Where(x => x.UserId == userId.ToString())
                        join r in _context.Roles on ur.RoleId equals r.Id
                        join a in _context.Applications on r.ApplicationId equals a.ApplicationId
                        select a;

            return await apps.Distinct().ToListAsync();
        }
    }
}
