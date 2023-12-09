using Microsoft.AspNetCore.Identity;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;
using System.Security.Claims;

namespace SSO.Infrastructure.Management
{
    public class UserClaimRepository : IUserClaimRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public UserClaimRepository(UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public Task<IEnumerable<Claim>> GetClaims(string userName, Guid applicationId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Claim>> GetClaims(Guid userId, Guid applicationId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            return _context.ApplicationUserClaims.Where(x => x.UserId == userId.ToString()
                        && x.Permission.ApplicationId == applicationId).Select(x => new Claim("permissions", x.ClaimValue!));
        }
    }
}
