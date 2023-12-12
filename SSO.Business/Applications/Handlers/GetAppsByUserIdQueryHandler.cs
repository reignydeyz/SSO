using AutoMapper;
using MediatR;
using SSO.Business.Applications;
using SSO.Business.ApplicationUsers.Queries;
using SSO.Domain.Management.Interfaces;

namespace SSO.Business.ApplicationUsers.Handlers
{
    public class GetAppsByUserIdQueryHandler : IRequestHandler<GetAppsByUserIdQuery, IEnumerable<ApplicationDto>>
    {
        readonly IApplicationRepository _applicationRepository;
        readonly IMapper _mapper;

        public GetAppsByUserIdQueryHandler(IApplicationRepository applicationRepository, IMapper mapper)
        {
            _applicationRepository = applicationRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ApplicationDto>> Handle(GetAppsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var res = await _applicationRepository.GetAppsByUserId(request.UserId);

            return _mapper.Map<List<ApplicationDto>>(res);
        }
    }
}
