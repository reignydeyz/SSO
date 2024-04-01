using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;

namespace SSO.Infrastructure.Management
{
    public class GroupRoleRepository : IGroupRoleRepository
    {
        readonly AppDbContext _context;

        public GroupRoleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task Add(GroupUser param, bool? saveChanges = true, object? args = null)
        {
            _context.Add(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }

        public async Task Delete(GroupUser param, bool? saveChanges = true, object? args = null)
        {
            _context.Remove(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }
    }
}
