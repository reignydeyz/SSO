using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;
using System.Linq.Expressions;

namespace SSO.Infrastructure.Management
{
    public class GroupRoleRepository : IGroupRoleRepository
    {
        readonly AppDbContext _context;

        public GroupRoleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddRange(IEnumerable<GroupRole> param, bool? saveChanges = true, object? args = null)
        {
            _context.AddRange(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }

        public async Task RemoveRange(IEnumerable<GroupRole> param, bool? saveChanges = true, object? args = null)
        {
            _context.RemoveRange(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }

        public async Task RemoveRange(Expression<Func<GroupRole, bool>> predicate, bool? saveChanges = true, object? args = null)
        {
            _context.RemoveRange(_context.GroupRoles.Where(predicate));

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }
    }
}
