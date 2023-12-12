using Microsoft.EntityFrameworkCore;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;
using System.Linq.Expressions;

namespace SSO.Infrastructure.Management
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly AppDbContext _context;

        public ApplicationRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<Application> Add(Application param)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Any(Expression<Func<Application, bool>> predicate)
        {
            return await _context.Applications.AnyAsync(predicate);
        }

        public Task Delete(Application param)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Application>> Find(Expression<Func<Application, bool>>? predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<Application> FindOne(Expression<Func<Application, bool>> predicate)
        {
            return await _context.Applications.FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<Application>> GetAppsByUserId(Guid userId)
        {
            var roleIds = _context.UserRoles.Where(x => x.UserId == userId.ToString())
                        .Select(x => x.RoleId);

            var apps = _context.Roles.Where(x => roleIds.Contains(x.Id.ToString())).Select(x => x.Application);

            return await apps.ToListAsync();
        }

        public async Task<IEnumerable<string>> GetCallbackUrls(Guid applicationId)
        {
            var res = _context.ApplicationCallbacks.Where(x => x.ApplicationId == applicationId).Select(x => x.Url);

            return await res.ToListAsync();
        }

        public Task<IEnumerable<string>> GetPermissions(Guid applicationId)
        {
            throw new NotImplementedException();
        }

        public Task<Application> Update(Application param)
        {
            throw new NotImplementedException();
        }
    }
}
