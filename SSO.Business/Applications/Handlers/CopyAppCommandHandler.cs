using AutoMapper;
using MediatR;
using SSO.Business.Applications.Commands;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;
using System.Data;

namespace SSO.Business.Applications.Handlers
{
    public class CopyAppCommandHandler : IRequestHandler<CopyAppCommand, ApplicationDetailDto>
    {
        readonly IApplicationRepository _applicationRepository;
        readonly IApplicationPermissionRepository _applicationPermissionRepository;
        readonly IApplicationCallbackRepository _applicationCallbackRepository;
        readonly IApplicationRoleRepository _applicationRoleRepository;
        readonly IApplicationRoleClaimRepository _applicationRoleClaimRepository;
        readonly IUserRoleRepository _userRoleRepository;
        readonly IMapper _mapper;

        public CopyAppCommandHandler(IApplicationRepository applicationRepository,
            IApplicationPermissionRepository applicationPermissionRepository,
            IApplicationCallbackRepository applicationCallbackRepository,
            IApplicationRoleRepository applicationRoleRepository,
            IApplicationRoleClaimRepository applicationRoleClaimRepository,
            IUserRoleRepository userRoleRepository,
            IMapper mapper)
        {
            _applicationRepository = applicationRepository;
            _applicationPermissionRepository = applicationPermissionRepository;
            _applicationCallbackRepository = applicationCallbackRepository;
            _applicationRoleRepository = applicationRoleRepository;
            _applicationRoleClaimRepository = applicationRoleClaimRepository;
            _userRoleRepository = userRoleRepository;
            _mapper = mapper;
        }

        public async Task<ApplicationDetailDto> Handle(CopyAppCommand request, CancellationToken cancellationToken)
        {
            var srcApp = await _applicationRepository.FindOne(x => x.ApplicationId == request.ApplicationId);

            var existingApp = await _applicationRepository.FindOne(x => x.Name == request.Name);

            if (existingApp != null)
                await _applicationRepository.Delete(existingApp);

            var newApp = _mapper.Map<Application>(request);
            newApp.ApplicationId = existingApp?.ApplicationId ?? Guid.NewGuid();
            newApp.MaxAccessFailedCount = srcApp.MaxAccessFailedCount;
            newApp.TokenExpiration = srcApp.TokenExpiration;
            newApp.RefreshTokenExpiration = srcApp.RefreshTokenExpiration;
            newApp.CreatedBy = existingApp?.CreatedBy ?? request.Author!;
            newApp.DateCreated = existingApp?.DateCreated ?? DateTime.Now;
            newApp.ModifiedBy = request.Author!;
            newApp.DateModified = DateTime.Now;

            // Callbacks
            var newCallbacks = (await _applicationRepository.GetCallbacks(request.ApplicationId)).Select(x => new ApplicationCallback {
                ApplicationId = newApp.ApplicationId,
                Url = x.Url,
            }).ToList();
            await _applicationCallbackRepository.AddRange(newCallbacks, false);

            // Permissions
            var newPermissions = (await _applicationRepository.GetPermissions(request.ApplicationId)).Select(x => new ApplicationPermission {
                ApplicationId = newApp.ApplicationId,
                PermissionId = Guid.NewGuid(),
                Name = x.Name,
                Description = x.Description
            }).ToList();
            await _applicationPermissionRepository.AddRange(newPermissions, false);

            // Roles
            var roles = (await _applicationRepository.GetRoles(request.ApplicationId));
            var newRoles = roles.Select(x => new ApplicationRole {
                ApplicationId = newApp.ApplicationId,
                Id = Guid.NewGuid().ToString(),
                Name = x.Name,
                NormalizedName = x.NormalizedName,
            }).ToList();
            await _applicationRoleRepository.AddRange(newRoles, false);

            // Role claims
            var roleClaims = new List<ApplicationRoleClaim>();
            foreach (var role in roles)
            {
                var claims = await _applicationRoleRepository.GetClaims(new Guid(role.Id));

                foreach (var claim in claims)
                {
                    roleClaims.Add(new ApplicationRoleClaim {
                        RoleId = newRoles.First(x => x.Name == role.Name).Id,
                        ClaimType = claim.Type,
                        ClaimValue = claim.Value,
                        PermissionId = newPermissions.First(x => x.Name == claim.Value).PermissionId
                    });
                }
            }
            await _applicationRoleClaimRepository.AddRange(roleClaims, false);

            // Users
            var users = (await _applicationRepository.GetUsers(request.ApplicationId)).ToList();
            foreach (var user in users)
            {
                var userRoles = await _userRoleRepository.Roles(new Guid(user.Id), request.ApplicationId);
                var newUserRoles = userRoles.Select(x => new ApplicationRole {
                    Id = newRoles.First(y => y.Name == x.Name).Id,
                });

                await _userRoleRepository.AddRoles(new Guid(user.Id), newUserRoles, false);
            }

            await _applicationRepository.Add(newApp);

            return _mapper.Map<ApplicationDetailDto>(newApp);
        }
    }
}
