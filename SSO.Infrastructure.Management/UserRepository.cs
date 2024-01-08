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

        public Task<ApplicationUser> Add(ApplicationUser param, object? args = null)
        {
            throw new NotImplementedException();
        }

        public Task Delete(ApplicationUser param)
        {
            throw new NotImplementedException();
        }

        public async Task<IQueryable<ApplicationUser>> Find(Expression<Func<ApplicationUser, bool>>? predicate)
        {
            return _context.ApplicationUsers.AsNoTracking();
        }

        public async Task<ApplicationUser> GetByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
                throw new ArgumentNullException();

            return user;
        }

        public async Task<ApplicationUser> FindOne(Expression<Func<ApplicationUser, bool>> predicate)
        {
            return await _context.ApplicationUsers.FirstOrDefaultAsync(predicate);
        }

        public Task<ApplicationUser> Update(ApplicationUser param, object? args = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Any(Expression<Func<ApplicationUser, bool>> predicate)
        {
            throw new NotImplementedException();
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
    }
}
