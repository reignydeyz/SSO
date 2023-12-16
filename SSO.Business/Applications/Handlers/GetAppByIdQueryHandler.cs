using AutoMapper;
using MediatR;
using SSO.Business.Applications.Queries;
using SSO.Domain.Management.Interfaces;

namespace SSO.Business.Applications.Handlers
{
    public class GetAppByIdQueryHandler : IRequestHandler<GetAppByIdQuery, ApplicationDto>
    {
        readonly IApplicationRepository _applicationRepository;
        readonly IMapper _mapper;

        public GetAppByIdQueryHandler(IApplicationRepository applicationRepository, IMapper mapper)
        {
            _applicationRepository = applicationRepository;
            _mapper = mapper;
        }

        public async Task<ApplicationDto> Handle(GetAppByIdQuery request, CancellationToken cancellationToken)
        {
            var res = await _applicationRepository.FindOne(x => x.ApplicationId == request.AppId);

            if (res is null)
                throw new ArgumentNullException();

            return _mapper.Map<ApplicationDto>(res);
        }
    }
}
