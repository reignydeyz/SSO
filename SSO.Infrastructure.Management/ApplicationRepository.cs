using Microsoft.EntityFrameworkCore;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;
using System.Linq.Expressions;

namespace SSO.Infrastructure.Management
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly AppDbContext _context;

        public ApplicationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Application> Add(Application param, bool? saveChanges = true, object? args = null)
        {
            _context.Add(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();

            return param;
        }

        public async Task<bool> Any(Expression<Func<Application, bool>> predicate)
        {
            return await _context.Applications.AnyAsync(predicate);
        }

        public async Task Delete(Application param, bool? saveChanges = true)
        {
            _context.Remove(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }

        public async Task<IQueryable<Application>> Find(Expression<Func<Application, bool>>? predicate)
        {
            return _context.Applications.AsQueryable().AsNoTracking();
        }

        public async Task<Application> FindOne(Expression<Func<Application, bool>> predicate)
        {
            return await _context.Applications.FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<Application>> GetAppsByUserId(Guid userId)
        {
            var roleIds = _context.UserRoles.Where(x => x.UserId == userId.ToString())
                        .Select(x => x.RoleId);

            var apps = _context.Roles.Where(x => roleIds.Contains(x.Id.ToString())).Select(x => x.Application);

            return await apps.ToListAsync();
        }

        public async Task<IEnumerable<ApplicationCallback>> GetCallbacks(Guid applicationId)
        {
            var res = _context.ApplicationCallbacks.Where(x => x.ApplicationId == applicationId);

            return await res.ToListAsync();
        }

        public async Task<IEnumerable<ApplicationPermission>> GetPermissions(Guid applicationId)
        {
            var res = _context.ApplicationPermissions.Where(x => x.ApplicationId == applicationId);

            return await res.ToListAsync();
        }

        public async Task<IEnumerable<ApplicationRole>> GetRoles(Guid applicationId)
        {
            var res = _context.ApplicationRoles.Where(x => x.ApplicationId == applicationId);

            return await res.ToListAsync();
        }

        public async Task<IQueryable<ApplicationUser>> GetUsers(Guid applicationId)
        {
            var res = from r in _context.Roles.Where(x => x.ApplicationId == applicationId)
                      join ur in _context.UserRoles on r.Id equals ur.RoleId
                      join u in _context.Users on ur.UserId equals u.Id
                      select u;

            return await Task.FromResult(res);
        }

        public async Task<Application> Update(Application param, bool? saveChanges = true, object? args = null)
        {
            var rec = await _context.Applications.FirstAsync(x => x.ApplicationId == param.ApplicationId);

            rec.Name = param.Name;
            rec.TokenExpiration = param.TokenExpiration;
            rec.RefreshTokenExpiration = param.RefreshTokenExpiration;
            rec.MaxAccessFailedCount = param.MaxAccessFailedCount;
            rec.ModifiedBy = param.ModifiedBy;
            rec.DateModified = DateTime.Now;

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();

            return rec;
        }
    }
}
