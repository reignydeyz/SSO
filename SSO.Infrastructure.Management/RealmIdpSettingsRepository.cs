using Microsoft.EntityFrameworkCore;
using SSO.Domain.Interfaces;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;
using System.Linq.Expressions;

namespace SSO.Infrastructure.Management
{
    public class RealmIdpSettingsRepository : IRealmIdpSettingsRepository
    {
        readonly IAppDbContext _context;

        public RealmIdpSettingsRepository(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<RealmIdpSettings> Add(RealmIdpSettings param, bool? saveChanges = true, object? args = null)
        {
            _context.Add(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();

            return param;
        }

        public Task<bool> Any(Expression<Func<RealmIdpSettings, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(RealmIdpSettings param, bool? saveChanges = true, object? args = null)
        {
            _context.Remove(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }

        public Task<IQueryable<RealmIdpSettings>> Find(Expression<Func<RealmIdpSettings, bool>>? predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<RealmIdpSettings> FindOne(Expression<Func<RealmIdpSettings, bool>> predicate)
        {
            return await _context.RealmIdpSettings.FirstOrDefaultAsync(predicate);
        }

        public async Task<RealmIdpSettings> Update(RealmIdpSettings param, bool? saveChanges = true, object? args = null)
        {
            var rec = await _context.RealmIdpSettings.FirstAsync(x => x.RealmId == param.RealmId && x.IdentityProvider == param.IdentityProvider);

            _context.Entry(rec).CurrentValues.SetValues(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();

            return rec;
        }
    }
}
