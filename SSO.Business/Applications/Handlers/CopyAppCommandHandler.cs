using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SSO.Business.Applications.Commands;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;
using System.Data;

namespace SSO.Business.Applications.Handlers
{
    public class CopyAppCommandHandler : IRequestHandler<CopyAppCommand, ApplicationDetailDto>
    {
        readonly IServiceProvider _serviceProvider;
        readonly IMapper _mapper;

        public CopyAppCommandHandler(IServiceProvider serviceProvider,
            IMapper mapper)
        {
            _serviceProvider = serviceProvider;
            _mapper = mapper;
        }

        public async Task<ApplicationDetailDto> Handle(CopyAppCommand request, CancellationToken cancellationToken)
        {
            var appRepo = _serviceProvider.GetService<IApplicationRepository>();

            var rec = _mapper.Map<Application>(request);
            rec.MaxAccessFailedCount = rec.MaxAccessFailedCount;
            rec.TokenExpiration = rec.TokenExpiration;
            rec.RefreshTokenExpiration = rec.RefreshTokenExpiration;
            rec.CreatedBy = request.Author;
            rec.DateCreated = DateTime.Now;
            rec.ModifiedBy = request.Author;
            rec.DateModified = rec.DateCreated;            

            // Callbacks
            var newCallbacks = (await appRepo.GetCallbacks(request.ApplicationId)).Select(x => new ApplicationCallback {
                ApplicationId = rec.ApplicationId,
                Url = x.Url,
            }).ToList();
            await _serviceProvider.GetService<IApplicationCallbackRepository>()!.AddRange(newCallbacks, false);

            // Permissions
            var newPermissions = (await appRepo.GetPermissions(request.ApplicationId)).Select(x => new ApplicationPermission {
                ApplicationId = rec.ApplicationId,
                PermissionId = Guid.NewGuid(),
                Name = x.Name,
                Description = x.Description
            }).ToList();
            await _serviceProvider.GetService<IApplicationPermissionRepository>()!.AddRange(newPermissions, false);

            // Roles
            var roles = (await appRepo.GetRoles(request.ApplicationId));
            var newRoles = roles.Select(x => new ApplicationRole {
                ApplicationId = rec.ApplicationId,
                Id = Guid.NewGuid().ToString(),
                Name = x.Name,
                NormalizedName = x.NormalizedName,
            }).ToList();
            var roleRepo = _serviceProvider.GetService<IApplicationRoleRepository>();
            await roleRepo!.AddRange(newRoles, false);

            // Role claims
            var roleClaims = new List<ApplicationRoleClaim>();
            foreach (var role in roles)
            {
                var claims = await roleRepo.GetClaims(new Guid(role.Id));

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
            await _serviceProvider.GetService<IApplicationRoleClaimRepository>()!.AddRange(roleClaims, false);

            // Users
            var users = (await appRepo.GetUsers(request.ApplicationId)).ToList();
            var userRoleRepo = _serviceProvider.GetService<IUserRoleRepository>();
            foreach (var user in users)
            {
                var userRoles = await userRoleRepo.Roles(new Guid(user.Id), request.ApplicationId);
                var newUserRoles = userRoles.Select(x => new ApplicationRole {
                    Id = newRoles.First(y => y.Name == x.Name).Id,
                });

                await userRoleRepo.AddRoles(new Guid(user.Id), newUserRoles, false);
            }

            await appRepo.Add(rec);

            return _mapper.Map<ApplicationDetailDto>(rec);
        }
    }
}
