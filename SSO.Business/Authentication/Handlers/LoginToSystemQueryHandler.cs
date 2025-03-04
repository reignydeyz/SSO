﻿using MediatR;
using SSO.Business.Authentication.Queries;
using SSO.Domain.Authentication.Interfaces;
using SSO.Domain.Management.Interfaces;
using SSO.Infrastructure.Settings.Enums;
using SSO.Infrastructure.Settings.Services;
using System.Security.Claims;
using System.Text;

namespace SSO.Business.Authentication.Handlers
{
    public class LoginToSystemQueryHandler : IRequestHandler<LoginToSystemQuery, TokenDto>
    {
        readonly ITokenService _tokenService;
        readonly IOtpService _otpService;
        readonly IApplicationRepository _applicationRepository;
        readonly IApplicationRoleRepository _roleRepo;
        readonly IUserRoleRepository _userRoleRepo;
        readonly IGroupRoleRepository _groupRoleRepo;
        readonly IRealmRepository _realmRepo;
        readonly ServiceFactory _authServiceFactory;
        readonly Users.RepositoryFactory _userRepoFactory;
        readonly JwtSecretService _jwtSecretService;
        readonly RsaKeyService _rsaKeyService;

        public LoginToSystemQueryHandler(ITokenService tokenService, IOtpService otpService,
            IApplicationRepository applicationRepository, IApplicationRoleRepository roleRepo, IUserRoleRepository userRoleRepo,
            IGroupRoleRepository groupRoleRepository,
            IRealmRepository realmRepository,
            ServiceFactory authServiceFactory, Users.RepositoryFactory userRepoFactory,
            JwtSecretService jwtSecretService, RsaKeyService rsaKeyService)
        {
            _tokenService = tokenService;
            _otpService = otpService;
            _applicationRepository = applicationRepository;
            _roleRepo = roleRepo;
            _userRoleRepo = userRoleRepo;
            _groupRoleRepo = groupRoleRepository;
            _realmRepo = realmRepository;
            _authServiceFactory = authServiceFactory;
            _userRepoFactory = userRepoFactory;
            _jwtSecretService = jwtSecretService;
            _rsaKeyService = rsaKeyService;
        }

        public async Task<TokenDto> Handle(LoginToSystemQuery request, CancellationToken cancellationToken)
        {
           var root = await _applicationRepository.FindOne(
                x => request.RealmId.HasValue ?
                        x.RealmId == request.RealmId && x.Name == "root" :
                        x.Realm.Name == "Default" && x.Name == "root"
            );

            if (root is null)
                throw new ArgumentException("Invalid realm.");

            var isLdap = await _realmRepo.Any(x => x.RealmId == root.RealmId && x.IdpSettingsCollection.Any(y => y.IdentityProvider == IdentityProvider.LDAP));
            var authenticationService = await _authServiceFactory.GetService(root.RealmId);
            var userRepo = await _userRepoFactory.GetRepository(root.RealmId);

            await authenticationService.Login(request.Username, request.Password, root);

            var user = await userRepo.GetByUsername(request.Username);

            if (user.TwoFactorEnabled)
            {
                if (string.IsNullOrEmpty(request.Otp))
                    throw new InvalidOperationException("OTP is required.");
                else
                {
                    var encSecretStr = Encoding.ASCII.GetString(user.TwoFactorSecret!);
                    var secret = _rsaKeyService.DecryptString(encSecretStr, _jwtSecretService.PrivateKey);

                    var validOtp = _otpService.VerifyOtp(secret, request.Otp);

                    if (!validOtp)
                        throw new UnauthorizedAccessException("Invalid OTP.");
                }
            }

            // Checks root access
            var roles = await _userRoleRepo.Roles(request.Username, root.ApplicationId);

            var groups = await userRepo.GetGroups(new Guid(user.Id));
            foreach (var group in groups)
                roles = roles.Union(await _groupRoleRepo.Roles(group.GroupId, root.ApplicationId));

            var claims = new List<Claim>() {
                new Claim(ClaimTypes.AuthenticationMethod, (isLdap ? IdentityProvider.LDAP.ToString() : IdentityProvider.Default.ToString())),
                new Claim(ClaimTypes.NameIdentifier, $"{user.Id}"),
                new Claim(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}"),
                new Claim("realm", root.RealmId.ToString()),
                new Claim("app", root.ApplicationId.ToString())
            };

            if (user.Email is not null)
                claims.Add(new Claim(ClaimTypes.Email, user.Email));

            // Has root access
            if (roles.Any())
            {
                foreach (var role in roles.ToList())
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Name!));

                    var permissions = await _roleRepo.GetClaims(new Guid(role.Id));

                    foreach (var p in permissions)
                        claims.Add(new Claim("permissions", p.Value));
                }
            }

            var expires = DateTime.Now.AddMinutes(root.TokenExpiration);
            var token = _tokenService.GenerateToken(new ClaimsIdentity(claims), expires);

            return new TokenDto { AccessToken = token, Expires = expires };
        }
    }
}
