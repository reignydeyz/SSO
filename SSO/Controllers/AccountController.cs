using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSO.Business.Accounts.Commands;
using SSO.Domain.Management.Interfaces;
using SSO.Infrastructure.Settings.Enums;
using SSO.Infrastructure.Settings.Services;
using System.Security.Claims;

namespace SSO.Controllers
{
    [ApiExplorerSettings(GroupName = "System")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        readonly IMediator _mediator;
        readonly IUserRepository _userRepository;
        readonly IMapper _mapper;
        readonly IdentityProvider _idp;

        public AccountController(IMediator mediator, IUserRepository userRepository, IMapper mapper, IdpService idpService)
        {
            _mediator = mediator;
            _userRepository = userRepository;
            _mapper = mapper;
            _idp = idpService.IdentityProvider;
        }

        /// <summary>
        /// Changes password
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand param)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                param.User = await _userRepository.FindOne(x => x.Id == userId);

                await (_idp switch
                {
                    IdentityProvider.Default => _mediator.Send(param),
                    IdentityProvider.LDAP => _mediator.Send(_mapper.Map<ChangePasswordLdapCommand>(param)),
                    _ => throw new InvalidOperationException("Unsupported IDP")
                });

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
