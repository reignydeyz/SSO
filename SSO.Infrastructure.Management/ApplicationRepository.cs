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

        public async Task<Application> Add(Application param, object? args)
        {
            _context.Add(param);

            await _context.SaveChangesAsync();

            return param;
        }

        public async Task<bool> Any(Expression<Func<Application, bool>> predicate)
        {
            return await _context.Applications.AnyAsync(predicate);
        }

        public Task Delete(Application param)
        {
            throw new NotImplementedException();
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

        public async Task<Application> Update(Application param, object? args = null)
        {
            var rec = await _context.Applications.FirstAsync(x => x.ApplicationId == param.ApplicationId);

            rec.Name = param.Name;
            rec.TokenExpiration = param.TokenExpiration;
            rec.RefreshTokenExpiration = param.RefreshTokenExpiration;
            rec.MaxAccessFailedCount = param.MaxAccessFailedCount;
            rec.ModifiedBy = param.ModifiedBy;
            rec.DateModified = DateTime.Now;

            _context.SaveChanges();

            return rec;
        }
    }
}
