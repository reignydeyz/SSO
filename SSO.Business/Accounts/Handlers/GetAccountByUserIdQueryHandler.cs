using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SSO.Business.Accounts.Queries;
using SSO.Domain.Models;

namespace SSO.Business.Accounts.Handlers
{
    public class GetAccountByUserIdQueryHandler : IRequestHandler<GetAccountByUserIdQuery, AccountDto>
    {
        readonly UserManager<ApplicationUser> _userManager;
        readonly IMapper _mapper;

        public GetAccountByUserIdQueryHandler(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<AccountDto> Handle(GetAccountByUserIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);

            return _mapper.Map<AccountDto>(user);
        }
    }
}
