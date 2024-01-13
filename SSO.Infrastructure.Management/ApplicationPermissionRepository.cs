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

        public Task<ApplicationPermission> Add(ApplicationPermission param, object? args = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Any(Expression<Func<ApplicationPermission, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task Delete(ApplicationPermission param)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<ApplicationPermission>> Find(Expression<Func<ApplicationPermission, bool>>? predicate)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationPermission> FindOne(Expression<Func<ApplicationPermission, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationPermission> Update(ApplicationPermission param, object? args = null)
        {
            throw new NotImplementedException();
        }
    }
}
