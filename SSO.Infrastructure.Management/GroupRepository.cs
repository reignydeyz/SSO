using Microsoft.EntityFrameworkCore;
using SSO.Domain.Interfaces;
using SSO.Domain.Models;

namespace SSO.Infrastructure.Management
{
    public class GroupRepository : GroupRepositoryBase
    {
        readonly IAppDbContext _context;

        public GroupRepository(IAppDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<Group> Add(Group param, bool? saveChanges = true, object? args = null)
        {
            _context.Add(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();

            return param;
        }

        public override async Task Delete(Group param, bool? saveChanges = true, object? args = null)
        {
            _context.Remove(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }

        public override async Task<Group> Update(Group param, bool? saveChanges = true, object? args = null)
        {
            var rec = await _context.Groups.FirstAsync(x => x.GroupId == param.GroupId);

            rec.Name = param.Name;
            rec.Description = param.Description;
            rec.ModifiedBy = param.ModifiedBy;
            rec.DateModified = DateTime.Now;

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();

            return rec;
        }
    }
}
