using Microsoft.AspNetCore.Identity;
using SSO.Domain.Models;
using SSO.Domain.UserManegement.Interfaces;
using System.Linq.Expressions;
using System.Security.Claims;

namespace SSO.Infrastructure.UserManagement
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

        public Task<ApplicationUser> Add(ApplicationUser param)
        {
            throw new NotImplementedException();
        }

        public Task Delete(ApplicationUser param)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApplicationUser>> Find(Expression<Func<ApplicationUser, bool>>? predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<ApplicationUser> GetByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
                throw new ArgumentNullException();

            return user;
        }

        public Task<ApplicationUser> FindOne(Expression<Func<ApplicationUser, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> Update(ApplicationUser param)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Claim>> GetClaims(string userName, Guid applicationId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Claim>> GetClaims(Guid userId, Guid applicationId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            return _context.ApplicationUserClaims.Where(x => x.UserId == userId.ToString()
                        && x.Permission.ApplicationId == applicationId).Select(x => new Claim(x.ClaimType!, x.ClaimValue!));
        }
    }
}
