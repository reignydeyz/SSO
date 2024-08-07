﻿using AutoMapper;
using MediatR;
using SSO.Business.Users.Commands;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;
using SSO.Infrastructure.Settings.Enums;
using SSO.Infrastructure.Settings.Services;

namespace SSO.Business.Users.Handlers
{
    public class CopyUserCommandHandler : IRequestHandler<CopyUserCommand, UserDto>
    {
        readonly IdentityProvider _idp;
        readonly IUserRepository _userRepository;
        readonly IUserRoleRepository _userRoleRepository;
        readonly IGroupUserRepository _groupUserRepository;
        readonly IMapper _mapper;

        public CopyUserCommandHandler(IdpService idpService,
            IUserRepository userRepository, 
            IUserRoleRepository userRoleRepository,
            IGroupUserRepository groupUserRepository,
            IMapper mapper)
        {
            _idp = idpService.IdentityProvider;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _groupUserRepository = groupUserRepository;
            _mapper = mapper;            
        }

        public async Task<UserDto> Handle(CopyUserCommand request, CancellationToken cancellationToken)
        {
            var srcUser = await _userRepository.FindOne(x => x.Id == request.UserId);

            ApplicationUser? existingUser = null;

            if (_idp == IdentityProvider.Default)
                existingUser = await _userRepository.FindOne(x => x.UserName == request.Username);
            else
            {
                existingUser = await _userRepository.FindOne(x => x.FirstName == request.FirstName
                    && x.LastName == request.LastName
                    && x.Email == request.Email
                    && x.UserName == request.Username);

                if (existingUser == null)
                    throw new ArgumentException(message: "Input may trigger \"core\" data modification which is currently not allowed.", paramName: "NotAllowed");
            }

            var newUser = _mapper.Map<ApplicationUser>(request);
            newUser.Id = existingUser?.Id ?? Guid.NewGuid().ToString();
            newUser.CreatedBy = existingUser?.CreatedBy ?? request.Author!;
            newUser.DateCreated = existingUser?.DateCreated ?? DateTime.Now;
            newUser.ModifiedBy = request.Author!;
            newUser.DateModified = DateTime.Now;

            if (_idp == IdentityProvider.Default)
            {
                if (existingUser == null)
                    await _userRepository.Add(newUser);
                else
                    await _userRepository.Update(newUser);
            }

            // Apps
            var apps = await _userRepository.GetApplications(new Guid(srcUser.Id));

            // Clear apps associated to the user being updated (existing user)
            if (existingUser != null)
            {
                var exApps = await _userRepository.GetApplications(new Guid(existingUser.Id));

                foreach (var app in exApps)
                {
                    var roles = await _userRoleRepository.Roles(new Guid(existingUser.Id), app.ApplicationId);
                    await _userRoleRepository.RemoveRoles(new Guid(existingUser.Id), roles);
                }
            }

            // User roles
            var newRoles = new List<ApplicationRole>();
            foreach (var app in apps)
            {
                var roles = await _userRoleRepository.Roles(new Guid(srcUser.Id), app.ApplicationId);

                if (existingUser != null)
                    await _userRoleRepository.RemoveRoles(new Guid(existingUser.Id), roles);

                newRoles.AddRange(roles);
            }
            await _userRoleRepository.AddRoles(new Guid(newUser.Id), newRoles);

            // User groups
            var groups = await _userRepository.GetGroups(new Guid(srcUser.Id));
            if (existingUser != null)
                await _groupUserRepository.RemoveRange(x => x.UserId == existingUser.Id, false);

            await _groupUserRepository.AddRange(groups.Select(x => new GroupUser { GroupId = x.GroupId, UserId = newUser.Id }));

            return _mapper.Map<UserDto>(newUser);
        }
    }
}
