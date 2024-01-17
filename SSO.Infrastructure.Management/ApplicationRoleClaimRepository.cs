using Microsoft.EntityFrameworkCore;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;
using System.Linq.Expressions;

namespace SSO.Infrastructure.Management
{
    public class ApplicationRoleClaimRepository : IApplicationRoleClaimRepository
    {
        readonly AppDbContext _context;

        public ApplicationRoleClaimRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<ApplicationRoleClaim> Add(ApplicationRoleClaim param, object? args = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Any(Expression<Func<ApplicationRoleClaim, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task Delete(ApplicationRoleClaim param)
        {
            throw new NotImplementedException();
        }

        public async Task<IQueryable<ApplicationRoleClaim>> Find(Expression<Func<ApplicationRoleClaim, bool>>? predicate)
        {
            if (predicate is not null)
                return await Task.Run(() => _context.ApplicationRoleClaims.Include(x => x.Permission).Where(predicate).AsQueryable().AsNoTracking());
            else
                return _context.ApplicationRoleClaims.Include(x => x.Permission).AsQueryable().AsNoTracking();
        }

        public Task<ApplicationRoleClaim> FindOne(Expression<Func<ApplicationRoleClaim, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationRoleClaim> Update(ApplicationRoleClaim param, object? args = null)
        {
            throw new NotImplementedException();
        }
    }
}
