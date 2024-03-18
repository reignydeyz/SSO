using Microsoft.EntityFrameworkCore;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;
using System.Linq.Expressions;

namespace SSO.Infrastructure.Management
{
    public class GroupRepository : IGroupRepository
    {
        readonly AppDbContext _context;

        public GroupRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Group> Add(Group param, bool? saveChanges = true, object? args = null)
        {
            _context.Add(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();

            return param;
        }

        public async Task AddRange(IEnumerable<Group> param, bool? saveChanges = true, object? args = null)
        {
            _context.AddRange(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }

        public async Task<bool> Any(Expression<Func<Group, bool>> predicate)
        {
            return await _context.Groups.AnyAsync(predicate);
        }

        public async Task Delete(Group param, bool? saveChanges = true)
        {
            _context.Remove(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }

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

        public Task RemoveRange(IEnumerable<Group> param, bool? saveChanges = true, object? args = null)
        {
            throw new NotImplementedException();
        }

        public Task RemoveRange(Expression<Func<Group, bool>> predicate, bool? saveChanges = true, object? args = null)
        {
            throw new NotImplementedException();
        }

        public async Task<Group> Update(Group param, bool? saveChanges = true, object? args = null)
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
