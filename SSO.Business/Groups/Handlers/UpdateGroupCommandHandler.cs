using AutoMapper;
using MediatR;
using SSO.Business.Groups.Commands;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;

namespace SSO.Business.Groups.Handlers
{
    public class UpdateGroupCommandHandler : IRequestHandler<UpdateGroupCommand, GroupDto>
    {
        readonly IGroupRepository _groupRepository;
        readonly IMapper _mapper;

        public UpdateGroupCommandHandler(IGroupRepository groupRepository, IMapper mapper)
        {
            _groupRepository  = groupRepository;
            _mapper = mapper;
        }

        public async Task<GroupDto> Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
        {
            var rec = _mapper.Map<Group>(request);
            rec.ModifiedBy = request.Author;
            rec.DateModified = rec.DateCreated;

            var res = await _groupRepository.Update(rec);

            return _mapper.Map<GroupDto>(res);
        }
    }
}
