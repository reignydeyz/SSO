using AutoMapper;
using MediatR;
using SSO.Business.Groups.Queries;
using SSO.Domain.Management.Interfaces;

namespace SSO.Business.Groups.Handlers
{
    public class GetGroupByIdQueryHandler : IRequestHandler<GetGroupByIdQuery, GroupDto>
    {
        readonly IGroupRepository _groupRepository;
        readonly IMapper _mapper;

        public GetGroupByIdQueryHandler(IGroupRepository groupRepository, IMapper mapper)
        {
            _groupRepository = groupRepository;
            _mapper = mapper;
        }

        public async Task<GroupDto> Handle(GetGroupByIdQuery request, CancellationToken cancellationToken)
        {
            var res = await _groupRepository.FindOne(x => x.GroupId == request.GroupId);

            if (res is null)
                throw new ArgumentNullException();

            return _mapper.Map<GroupDto>(res);
        }
    }
}
