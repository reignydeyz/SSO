using Microsoft.EntityFrameworkCore;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;
using System.Linq.Expressions;

namespace SSO.Infrastructure.Management
{
    public class ApplicationCallbackRepository : IApplicationCallbackRepository
    {
        readonly AppDbContext _context;

        public ApplicationCallbackRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApplicationCallback> Add(ApplicationCallback param, bool? saveChanges = true, object? args = null)
        {
            _context.Add(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();

            return param;
        }

        public async Task AddRange(IEnumerable<ApplicationCallback> param, bool? saveChanges = true, object? args = null)
        {
            _context.AddRange(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }

        public Task<bool> Any(Expression<Func<ApplicationCallback, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(ApplicationCallback param, bool? saveChanges = true)
        {
            _context.Remove(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }

        public Task<IQueryable<ApplicationCallback>> Find(Expression<Func<ApplicationCallback, bool>>? predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<ApplicationCallback> FindOne(Expression<Func<ApplicationCallback, bool>> predicate)
        {
            return await _context.ApplicationCallbacks.FirstOrDefaultAsync(predicate);
        }

        public Task RemoveRange(IEnumerable<ApplicationCallback> param, bool? saveChanges = true, object? args = null)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationCallback> Update(ApplicationCallback param, bool? saveChanges = true, object? args = null)
        {
            throw new NotImplementedException();
        }
    }
}
