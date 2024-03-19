using Microsoft.EntityFrameworkCore;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;
using System.Linq;
using System.Linq.Expressions;

namespace SSO.Infrastructure.Management
{
    public class GroupUserRepository : IGroupUserRepository
    {
        readonly AppDbContext _context;

        public GroupUserRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<GroupUser> Add(GroupUser param, bool? saveChanges = true, object? args = null)
        {
            throw new NotImplementedException();
        }

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

        public Task Delete(GroupUser param, bool? saveChanges = true)
        {
            throw new NotImplementedException();
        }

        public async Task<IQueryable<GroupUser>> Find(Expression<Func<GroupUser, bool>>? predicate)
        {
            if (predicate is not null)
                return await Task.Run(() => _context.GroupUsers.Where(predicate).AsQueryable().AsNoTracking());
            else
                return _context.GroupUsers.AsQueryable().AsNoTracking();
        }

        public Task<GroupUser> FindOne(Expression<Func<GroupUser, bool>> predicate)
        {
            throw new NotImplementedException();
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
