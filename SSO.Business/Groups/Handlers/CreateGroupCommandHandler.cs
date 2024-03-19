using AutoMapper;
using MediatR;
using SSO.Business.Groups.Commands;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;

namespace SSO.Business.Groups.Handlers
{
    public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, GroupDto>
    {
        readonly IGroupRepository _groupRepository;
        readonly IMapper _mapper;

        public CreateGroupCommandHandler(IGroupRepository groupRepository, IMapper mapper)
        {
            _groupRepository = groupRepository;
            _mapper = mapper;
        }

        public async Task<GroupDto> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
        {
            var rec = _mapper.Map<Group>(request);
            rec.CreatedBy = request.Author;
            rec.DateCreated = DateTime.Now;
            rec.ModifiedBy = request.Author;
            rec.DateModified = rec.DateCreated;

            var res = await _groupRepository.Add(rec);

            return _mapper.Map<GroupDto>(res);
        }
    }
}
