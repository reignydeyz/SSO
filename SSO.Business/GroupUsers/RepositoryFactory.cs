using Microsoft.Extensions.DependencyInjection;
using SSO.Domain.Management.Interfaces;
using SSO.Infrastructure.Settings.Enums;

namespace SSO.Business.GroupUsers
{
    public class RepositoryFactory
    {
        readonly IRealmRepository _realmRepository;
        readonly IServiceProvider _serviceProvider;

        public RepositoryFactory(IRealmRepository realmRepository, IServiceProvider serviceProvider)
        {
            _realmRepository = realmRepository;
            _serviceProvider = serviceProvider;
        }

        public async Task<IGroupUserRepository> GetRepository(Guid? realmId = null)
        {
            if (realmId.HasValue && await _realmRepository.Any(x => x.RealmId == realmId && x.IdpSettingsCollection.Any(x => x.IdentityProvider == IdentityProvider.LDAP)))
                return _serviceProvider.GetRequiredService<Infrastructure.LDAP.GroupUserRepository>();

            return _serviceProvider.GetRequiredService<Infrastructure.Management.GroupUserRepository>();
        }
    }
}
