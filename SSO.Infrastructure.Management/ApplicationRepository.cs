using Microsoft.EntityFrameworkCore;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;

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

        public Task Delete(Application param)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Application>> Find(Func<Application, bool>? predicate)
        {
            throw new NotImplementedException();
        }

        public Task<Application> FindOne(Func<Application, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<List<string>> GetAllowedOrigins(Guid? applicationId)
        {
            var origins = _context.ApplicationAllowedOrigins.AsQueryable();

            if (applicationId.HasValue)
                origins = origins.Where(x => x.ApplicationId == applicationId);

            return await origins.Select(x => x.Origin).ToListAsync();
        }

        public Task<string> GetPermissions(Guid applicationId)
        {
            throw new NotImplementedException();
        }

        public Task<Application> Update(Application param)
        {
            throw new NotImplementedException();
        }
    }
}
