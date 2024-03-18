using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;
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

        public async Task AddRange(IEnumerable<GroupUser> param, bool? saveChanges = true, object? args = null)
        {
            _context.AddRange(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
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
    }
}
