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

        public Task<RealmUser> Add(RealmUser param, bool? saveChanges = true, object? args = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Any(Expression<Func<RealmUser, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task Delete(RealmUser param, bool? saveChanges = true)
        {
            throw new NotImplementedException();
        }

        public async Task<IQueryable<RealmUser>> Find(Expression<Func<RealmUser, bool>>? predicate)
        {
            if (predicate is not null)
                return await Task.Run(() => _context.RealmUsers.Include(x => x.Realm)
                    .Where(predicate).AsQueryable().AsNoTracking());
            else
                return _context.RealmUsers.AsQueryable().AsNoTracking();
        }

        public Task<RealmUser> FindOne(Expression<Func<RealmUser, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<RealmUser> Update(RealmUser param, bool? saveChanges = true, object? args = null)
        {
            throw new NotImplementedException();
        }
    }
}
