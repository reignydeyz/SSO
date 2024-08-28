using Microsoft.EntityFrameworkCore;
using SSO.Domain.Interfaces;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;
using System.Linq.Expressions;

namespace SSO.Infrastructure.Management
{
    public class RealmUserRepository : IRealmUserRepository
    {
        readonly IAppDbContext _context;

        public RealmUserRepository(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<RealmUser> Add(RealmUser param, bool? saveChanges = true, object? args = null)
        {
            _context.Add(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();

            return param;
        }

        public async Task AddRange(IEnumerable<RealmUser> param, bool? saveChanges = true, object? args = null)
        {
            _context.AddRange(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }

        public async Task<bool> Any(Expression<Func<RealmUser, bool>> predicate)
        {
            return await _context.RealmUsers.AnyAsync(predicate);
        }

        public async Task Delete(RealmUser param, bool? saveChanges = true, object? args = null)
        {
            _context.Remove(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }

        public async Task<IQueryable<RealmUser>> Find(Expression<Func<RealmUser, bool>>? predicate)
        {
            if (predicate is not null)
                return await Task.Run(() => _context.RealmUsers
                    .Include(x => x.Realm)
                        .Where(predicate).AsQueryable().AsNoTracking());
            else
                return _context.RealmUsers.AsQueryable().AsNoTracking();
        }

        public async Task<RealmUser> FindOne(Expression<Func<RealmUser, bool>> predicate)
        {
            return await _context.RealmUsers.FirstOrDefaultAsync(predicate);
        }

        public async Task RemoveRange(IEnumerable<RealmUser> param, bool? saveChanges = true, object? args = null)
        {
            _context.RemoveRange(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }

        public async Task RemoveRange(Expression<Func<RealmUser, bool>> predicate, bool? saveChanges = true, object? args = null)
        {
            _context.RemoveRange(_context.RealmUsers.Where(predicate));

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }

        public Task<RealmUser> Update(RealmUser param, bool? saveChanges = true, object? args = null)
        {
            throw new NotImplementedException();
        }
    }
}
