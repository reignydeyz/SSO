using MediatR;
using Microsoft.Extensions.Caching.Memory;
using SSO.Business.Authentication.Queries;
using SSO.Domain.Authentication.Interfaces;
using SSO.Domain.Management.Interfaces;
using SSO.Infrastructure.Helpers;
using System.Security.Claims;

namespace SSO.Business.Authentication.Handlers
{
    /// <summary>
    /// Handler for login request
    /// </summary>
    public class LoginQueryHandler : IRequestHandler<LoginQuery, TokenDto>
    {
        readonly ITokenService _tokenService;
        readonly IOtpService _otpService;
        readonly IApplicationRepository _appRepo;
        readonly IApplicationRoleRepository _roleRepo;
        readonly IUserRoleRepository _userRoleRepo;
        readonly IGroupRoleRepository _groupRoleRepo;
        readonly IMemoryCache _cache;
        readonly ServiceFactory _authServiceFactory;
        readonly Users.RepositoryFactory _userRepoFactory;
        readonly MemoryCacheEntryOptions _cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(5));

        public LoginQueryHandler(ITokenService tokenService, IOtpService otpService, IApplicationRepository appRepo, IApplicationRoleRepository roleRepo, 
            IUserRoleRepository userRoleRepo, IGroupRoleRepository groupRoleRepo,
            IMemoryCache cache,
            ServiceFactory authServiceFactory, Users.RepositoryFactory userRepoFactory)
        {
            _tokenService = tokenService;
            _otpService = otpService;
            _appRepo = appRepo;
            _roleRepo = roleRepo;
            _userRoleRepo = userRoleRepo;
            _groupRoleRepo = groupRoleRepo;
            _authServiceFactory = authServiceFactory;
            _userRepoFactory = userRepoFactory;
            _cache = cache;
        }

        public async Task<TokenDto> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var app = await _appRepo.FindOne(x => x.ApplicationId == request.ApplicationId);

            var authenticationService = await _authServiceFactory.GetService(app.RealmId);
            var userRepo = await _userRepoFactory.GetRepository(app.RealmId);

            await authenticationService.Login(request.Username, request.Password, app);

            var user = await userRepo.GetByUsername(request.Username);

            if (user.TwoFactorEnabled)
            {
                if (string.IsNullOrEmpty(request.Otp))
                    throw new InvalidOperationException("OTP is required.");
                else
                {
                    var secret = CryptographyHelper.DecryptStringFromBytes_Aes(user.TwoFactorSecret!, user.TwoFactorSecretKey!);
                    var validOtp = _otpService.VerifyOtp(secret, request.Otp);

                    if (!validOtp)
                        throw new UnauthorizedAccessException("Invalid OTP.");
                }
            }

            var roles = await _userRoleRepo.Roles(request.Username, request.ApplicationId!.Value);

            var groups = await userRepo.GetGroups(new Guid(user.Id));
            foreach (var group in groups)
                roles = roles.Union(await _groupRoleRepo.Roles(group.GroupId, app.ApplicationId));

            var claims = new List<Claim>() {
                new Claim(ClaimTypes.NameIdentifier, $"{user.Id}"),
                new Claim(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}"),
                new Claim("realm", app.RealmId.ToString()),
                new Claim("app", app.ApplicationId.ToString())
            };

            if (!roles.Any())
                throw new UnauthorizedAccessException();

            foreach (var role in roles.ToList())
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name!));

                var permissions = await _roleRepo.GetClaims(new Guid(role.Id));

                foreach (var p in permissions)
                    claims.Add(new Claim("permissions", p.Value));
            }

            var expires = DateTime.Now.AddMinutes(app.TokenExpiration);
            var token = _tokenService.GenerateToken(new ClaimsIdentity(claims), expires, app.Name);

            var id = Guid.NewGuid();
            _cache.Set(id.ToString(), token, _cacheEntryOptions);

            return new TokenDto { Id = id, AccessToken = token, Expires = expires };
        }
    }
}
