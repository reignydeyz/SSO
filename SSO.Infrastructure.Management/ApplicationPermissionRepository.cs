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

        public async Task<ApplicationPermission> Add(ApplicationPermission param, object? args = null)
        {
            _context.Add(param);

            await _context.SaveChangesAsync();

            return param;
        }

        public Task<bool> Any(Expression<Func<ApplicationPermission, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(ApplicationPermission param)
        {
            _context.Remove(param);

            await _context.SaveChangesAsync();
        }

        public Task<IQueryable<ApplicationPermission>> Find(Expression<Func<ApplicationPermission, bool>>? predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<ApplicationPermission> FindOne(Expression<Func<ApplicationPermission, bool>> predicate)
        {
            return await _context.ApplicationPermissions.FirstOrDefaultAsync(predicate);
        }

        public Task<ApplicationPermission> Update(ApplicationPermission param, object? args = null)
        {
            throw new NotImplementedException();
        }
    }
}
