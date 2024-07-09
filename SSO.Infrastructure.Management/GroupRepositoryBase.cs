using Microsoft.EntityFrameworkCore;
using SSO.Domain.Interfaces;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;
using System.Linq.Expressions;

namespace SSO.Infrastructure.Management
{
    public abstract class GroupRepositoryBase : IGroupRepository
    {
        readonly IAppDbContext _context;

        protected GroupRepositoryBase(IAppDbContext context)
        {
            _context = context;
        }

        public abstract Task<Group> Add(Group param, bool? saveChanges = true, object? args = null);

        public async Task AddRange(IEnumerable<Group> param, bool? saveChanges = true, object? args = null)
        {
            await _context.AddRangeAsync(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }

        public async Task<bool> Any(Expression<Func<Group, bool>> predicate)
        {
            return await _context.Groups.AnyAsync(predicate);
        }

        public abstract Task Delete(Group param, bool? saveChanges = true);

        public async Task<IQueryable<Group>> Find(Expression<Func<Group, bool>>? predicate)
        {
            if (predicate is not null)
                return await Task.Run(() => _context.Groups.Where(predicate).AsQueryable().AsNoTracking());
            else
                return _context.Groups.AsQueryable().AsNoTracking();
        }

        public async Task<Group> FindOne(Expression<Func<Group, bool>> predicate)
        {
            return await _context.Groups.FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<Application>> GetApplications(Guid groupId)
        {
            var apps = from ur in _context.GroupRoles.Where(x => x.GroupId == groupId)
                       join r in _context.Roles on ur.RoleId equals r.Id
                       join a in _context.Applications on r.ApplicationId equals a.ApplicationId
                       select a;

            return await apps.Distinct().ToListAsync();
        }

        public async Task RemoveRange(IEnumerable<Group> param, bool? saveChanges = true, object? args = null)
        {
            _context.RemoveRange(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }

        public async Task RemoveRange(Expression<Func<Group, bool>> predicate, bool? saveChanges = true, object? args = null)
        {
            _context.RemoveRange(_context.Groups.Where(predicate));

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }

        public abstract Task<Group> Update(Group param, bool? saveChanges = true, object? args = null);
    }
}
