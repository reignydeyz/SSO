using Microsoft.EntityFrameworkCore;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;
using System.Linq.Expressions;

namespace SSO.Infrastructure.Management
{
    public class ApplicationPermissionRepository : IApplicationPermissionRepository
    {
        readonly AppDbContext _context;

        public ApplicationPermissionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApplicationPermission> Add(ApplicationPermission param, bool? saveChanges = true, object? args = null)
        {
            _context.Add(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();

            return param;
        }

        public async Task AddRange(IEnumerable<ApplicationPermission> param, bool? saveChanges = true, object? args = null)
        {
            _context.AddRange(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }

        public async Task<bool> Any(Expression<Func<ApplicationPermission, bool>> predicate)
        {
            return await _context.ApplicationPermissions.AnyAsync(predicate);
        }

        public async Task Delete(ApplicationPermission param, bool? saveChanges = true)
        {
            _context.RemoveRange(_context.ApplicationRoleClaims.Where(x => x.PermissionId == param.PermissionId));

            _context.Remove(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }

        public async Task<IQueryable<ApplicationPermission>> Find(Expression<Func<ApplicationPermission, bool>>? predicate)
        {
            if (predicate is not null)
                return await Task.Run(() => _context.ApplicationPermissions.Where(predicate).AsQueryable().AsNoTracking());
            else
                return _context.ApplicationPermissions.AsQueryable().AsNoTracking();
        }

        public async Task<ApplicationPermission> FindOne(Expression<Func<ApplicationPermission, bool>> predicate)
        {
            return await _context.ApplicationPermissions.FirstOrDefaultAsync(predicate);
        }

        public Task RemoveRange(IEnumerable<ApplicationPermission> param, bool? saveChanges = true, object? args = null)
        {
            throw new NotImplementedException();
        }

        public Task RemoveRange(Expression<Func<ApplicationPermission, bool>> predicate, bool? saveChanges = true, object? args = null)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationPermission> Update(ApplicationPermission param, bool? saveChanges = true, object? args = null)
        {
            throw new NotImplementedException();
        }
    }
}
