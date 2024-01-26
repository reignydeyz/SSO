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

        public async Task AddClaims(Guid userId, IEnumerable<ApplicationPermission> claims, bool? saveChanges = true)
        {
            var entries = claims.Select(x => new ApplicationUserClaim { 
                ClaimType = "Permission",
                ClaimValue = x.Name,
                UserId = userId.ToString(),
                PermissionId = x.PermissionId
            });

            _context.ApplicationUserClaims.AddRange(entries);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }

        public async Task AddClaims(IEnumerable<ApplicationUser> users, IEnumerable<ApplicationPermission> claims, bool? saveChanges = true)
        {
            foreach (var u in users)
            {
                var entries = claims.Select(x => new ApplicationUserClaim
                {
                    ClaimType = "Permission",
                    ClaimValue = x.Name,
                    UserId = u.Id.ToString(),
                    PermissionId = x.PermissionId
                });

                _context.ApplicationUserClaims.AddRange(entries);
            }

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Claim>> GetClaims(string userName, Guid applicationId)
        {
            var user = await _userManager.FindByNameAsync(userName) ?? throw new ArgumentNullException();

            return _context.ApplicationUserClaims.Where(x => x.UserId == user.Id
                        && x.Permission.ApplicationId == applicationId).Select(x => new Claim("permissions", x.ClaimValue!));
        }

        public async Task<IEnumerable<Claim>> GetClaims(Guid userId, Guid applicationId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString()) ?? throw new ArgumentNullException();

            return _context.ApplicationUserClaims.Where(x => x.UserId == userId.ToString()
                        && x.Permission.ApplicationId == applicationId).Select(x => new Claim("permissions", x.ClaimValue!));
        }

        public async Task RemoveClaims(Guid userId, IEnumerable<ApplicationPermission> claims, bool? saveChanges = true)
        {
            var entries = _context.ApplicationUserClaims.Where(x => claims.Any(y => y.PermissionId == x.PermissionId));

            _context.ApplicationUserClaims.RemoveRange(entries);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }

        public async Task RemoveClaims(IEnumerable<ApplicationUser> users, IEnumerable<ApplicationPermission> claims, bool? saveChanges = true)
        {
            var permissionIds = claims.Select(x => x.PermissionId);
            var userIds = users.Select(x => x.Id);

            var entries = _context.ApplicationUserClaims.Where(x => permissionIds.Contains(x.PermissionId)
                && userIds.Contains(x.UserId));

            _context.ApplicationUserClaims.RemoveRange(entries);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }
    }
}
