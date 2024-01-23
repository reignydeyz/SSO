using AutoMapper;
using MediatR;
using SSO.Business.ApplicationUsers.Queries;
using SSO.Business.Users;
using SSO.Domain.Management.Interfaces;

namespace SSO.Business.ApplicationUsers.Handlers
{
    public class GetUsersByApplicationIdQueryHandler : IRequestHandler<GetUsersByApplicationIdQuery, IQueryable<UserDto>>
    {
        readonly IApplicationRepository _applicationRepository;
        readonly IMapper _mapper;

        public GetUsersByApplicationIdQueryHandler(IApplicationRepository applicationRepository, IMapper mapper)
        {
            _applicationRepository = applicationRepository;
            _mapper = mapper;
        }

        public async Task<IQueryable<UserDto>> Handle(GetUsersByApplicationIdQuery request, CancellationToken cancellationToken)
        {
            var res = await _applicationRepository.GetUsers(request.ApplicationId);

            return _mapper.ProjectTo<UserDto>(res);
        }
    }
}
