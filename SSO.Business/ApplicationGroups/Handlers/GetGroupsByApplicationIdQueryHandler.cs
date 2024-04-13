using AutoMapper;
using MediatR;
using SSO.Business.ApplicationGroups.Queries;
using SSO.Business.Groups;
using SSO.Domain.Management.Interfaces;

namespace SSO.Business.ApplicationGroups.Handlers
{
    public class GetGroupsByApplicationIdQueryHandler : IRequestHandler<GetGroupsByApplicationIdQuery, IQueryable<GroupDto>>
    {
        readonly IApplicationRepository _applicationRepository;
        readonly IMapper _mapper;

        public GetGroupsByApplicationIdQueryHandler(IApplicationRepository applicationRepository, IMapper mapper)
        {
            _applicationRepository = applicationRepository;
            _mapper = mapper;
        }

        public async Task<IQueryable<GroupDto>> Handle(GetGroupsByApplicationIdQuery request, CancellationToken cancellationToken)
        {
            var res = await _applicationRepository.GetGroups(request.ApplicationId);

            return _mapper.ProjectTo<GroupDto>(res);
        }
    }
}
