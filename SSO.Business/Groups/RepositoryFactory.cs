﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SSO.Domain.Management.Interfaces;
using SSO.Infrastructure.Settings.Enums;

namespace SSO.Business.Groups
{
    public class RepositoryFactory
    {
        readonly IRealmRepository _realmRepository;
        readonly IServiceProvider _serviceProvider;
        readonly IConfiguration _config;

        public RepositoryFactory(IRealmRepository realmRepository, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _realmRepository = realmRepository;
            _serviceProvider = serviceProvider;
            _config = configuration;
        }

        public async Task<IGroupRepository> GetRepository(Guid? realmId = null)
        {
            if (realmId.HasValue && await _realmRepository.Any(x => x.RealmId == realmId && x.IdpSettingsCollection.Any(x => x.IdentityProvider == IdentityProvider.LDAP)))
                return _serviceProvider.GetRequiredService<Infrastructure.LDAP.GroupRepository>();

            return _serviceProvider.GetRequiredService<Infrastructure.Management.GroupRepository>();
        }
    }
}