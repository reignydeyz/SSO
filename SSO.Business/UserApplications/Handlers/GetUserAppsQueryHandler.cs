using AutoMapper;
using MediatR;
using SSO.Business.Applications;
using SSO.Business.UserApplications.Queries;
using SSO.Domain.Management.Interfaces;

namespace SSO.Business.UserApplications.Handlers
{
    public class GetUserAppsQueryHandler : IRequestHandler<GetUserAppsQuery, IEnumerable<ApplicationDto>>
    {
        readonly IUserRepository _userRepository;
        readonly IMapper _mapper;

        public GetUserAppsQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ApplicationDto>> Handle(GetUserAppsQuery request, CancellationToken cancellationToken)
        {
            var res = await _userRepository.GetApplications(request.UserId);

            return _mapper.Map<IEnumerable<ApplicationDto>>(res.OrderBy(x => x.Name));
        }
    }
}
