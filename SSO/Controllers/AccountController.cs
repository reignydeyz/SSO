using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSO.Business.Accounts.Commands;
using SSO.Domain.Management.Interfaces;
using SSO.Infrastructure.Settings.Enums;

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

        public AccountController(IMediator mediator, IUserRepository userRepository, IMapper mapper)
        {
            _mediator = mediator;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Changes password
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand param)
        {
            throw new NotImplementedException();
        }
    }
}
