using Microsoft.AspNetCore.Identity;
using SSO.Domain.Models;

namespace SSO.Infrastructure.Management
{
    public class UserRepository : UserRepositoryBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public UserRepository(UserManager<ApplicationUser> userManager, AppDbContext context) : base(userManager, context)
        {
            _userManager = userManager;
            _context = context;
        }

        public override async Task<ApplicationUser> Add(ApplicationUser param, bool? saveChanges = true, object? args = null)
        {
            var password = param.PasswordHash ?? Guid.NewGuid().ToString();
            param.PasswordHash = null;

            var res = await _userManager.CreateAsync(param, password);

            if (!res.Succeeded && res.Errors.Any())
                throw new ArgumentException(res.Errors.First().Description);

            return await _userManager.FindByNameAsync(param.UserName);
        }

        public override async Task Delete(ApplicationUser param, bool? saveChanges = true)
        {
            await _userManager.DeleteAsync(param);
        }

        public override async Task<ApplicationUser> Update(ApplicationUser param, bool? saveChanges = true, object? args = null)
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

            // Unlock the user
            await _userManager.ResetAccessFailedCountAsync(rec);
            await _userManager.SetLockoutEndDateAsync(rec, DateTimeOffset.MinValue);

            return await _userManager.FindByNameAsync(param.UserName);
        }

        public override async Task ChangePassword(ApplicationUser applicationUser, string password, ApplicationUser? author)
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
