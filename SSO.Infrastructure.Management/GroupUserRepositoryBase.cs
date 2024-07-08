using Microsoft.EntityFrameworkCore;
using SSO.Domain.Interfaces;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;
using System.Linq.Expressions;

namespace SSO.Infrastructure.Management
{
    public abstract class GroupUserRepositoryBase : IGroupUserRepository
    {
        readonly IAppDbContext _context;

        public GroupUserRepositoryBase(IAppDbContext context)
        {
            _context = context;
        }

        public abstract Task<GroupUser> Add(GroupUser param, bool? saveChanges = true, object? args = null);

        public async Task AddRange(IEnumerable<GroupUser> param, bool? saveChanges = true, object? args = null)
        {
            _context.AddRange(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }

        public async Task<bool> Any(Expression<Func<GroupUser, bool>> predicate)
        {
            return await _context.GroupUsers.AnyAsync(predicate);
        }

        public abstract Task Delete(GroupUser param, bool? saveChanges = true);

        public async Task<IQueryable<GroupUser>> Find(Expression<Func<GroupUser, bool>>? predicate)
        {
            if (predicate is not null)
                return await Task.Run(() => _context.GroupUsers.Where(predicate).AsQueryable().AsNoTracking());
            else
                return _context.GroupUsers.AsQueryable().AsNoTracking();
        }

        public async Task<GroupUser> FindOne(Expression<Func<GroupUser, bool>> predicate)
        {
            return await _context.GroupUsers.FirstOrDefaultAsync(predicate);
        }

        public async Task RemoveRange(IEnumerable<GroupUser> param, bool? saveChanges = true, object? args = null)
        {
            _context.RemoveRange(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }

        public async Task RemoveRange(Expression<Func<GroupUser, bool>> predicate, bool? saveChanges = true, object? args = null)
        {
            _context.RemoveRange(_context.GroupUsers.Where(predicate));

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }

        public Task<GroupUser> Update(GroupUser param, bool? saveChanges = true, object? args = null)
        {
            throw new NotImplementedException();
        }
    }
}
