using Microsoft.EntityFrameworkCore;
using SSO.Domain.Interfaces;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;
using System.Linq.Expressions;

namespace SSO.Infrastructure.Management
{
    public class ApplicationRoleClaimRepository : IApplicationRoleClaimRepository
    {
        readonly IAppDbContext _context;

        public ApplicationRoleClaimRepository(IAppDbContext context)
        {
            _context = context;
        }

        public Task<ApplicationRoleClaim> Add(ApplicationRoleClaim param, bool? saveChanges = true, object? args = null)
        {
            throw new NotImplementedException();
        }

        public async Task AddRange(IEnumerable<ApplicationRoleClaim> param, bool? saveChanges = true, object? args = null)
        {
            _context.AddRange(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }

        public async Task<bool> Any(Expression<Func<ApplicationRoleClaim, bool>> predicate)
        {
            return await _context.ApplicationRoleClaims.AnyAsync(predicate);
        }

        public Task Delete(ApplicationRoleClaim param, bool? saveChanges = true, object? args = null)
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

        public async Task RemoveRange(IEnumerable<ApplicationRoleClaim> param, bool? saveChanges = true, object? args = null)
        {
            _context.RemoveRange(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }

        public Task RemoveRange(Expression<Func<ApplicationRoleClaim, bool>> predicate, bool? saveChanges = true, object? args = null)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationRoleClaim> Update(ApplicationRoleClaim param, bool? saveChanges = true, object? args = null)
        {
            throw new NotImplementedException();
        }
    }
}
