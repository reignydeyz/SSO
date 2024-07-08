using SSO.Domain.Interfaces;
using SSO.Domain.Models;

namespace SSO.Infrastructure.Management
{
    public class GroupUserRepository : GroupUserRepositoryBase
    {
        readonly IAppDbContext _context;

        public GroupUserRepository(IAppDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<GroupUser> Add(GroupUser param, bool? saveChanges = true, object? args = null)
        {
            _context.Add(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();

            return param;
        }

        public override async Task Delete(GroupUser param, bool? saveChanges = true)
        {
            _context.Remove(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }
    }
}
