﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SSO.Domain.Authentication.Interfaces;
using SSO.Domain.Management.Interfaces;
using SSO.Infrastructure.Settings.Enums;

namespace SSO.Business.Authentication
{
    public class ServiceFactory
    {
        readonly IRealmRepository _realmRepository;
        readonly IServiceProvider _serviceProvider;
        readonly IConfiguration _config;

        public ServiceFactory(IRealmRepository realmRepository, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _realmRepository = realmRepository;
            _serviceProvider = serviceProvider;
            _config = configuration;
        }

        public async Task<IAuthenticationService> GetService(Guid? realmId)
        {
            if (realmId.HasValue && await _realmRepository.Any(x => x.RealmId == realmId && x.IdpSettingsCollection.Any(x => x.IdentityProvider == IdentityProvider.LDAP)))
                return _serviceProvider.GetRequiredService<Infrastructure.LDAP.AuthenticationService>();

            return _serviceProvider.GetRequiredService<Infrastructure.Authentication.AuthenticationService>();
        }
    }
}
