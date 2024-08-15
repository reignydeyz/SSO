using AutoMapper;
using MediatR;
using SSO.Business.Groups.Commands;
using SSO.Domain.Models;

namespace SSO.Business.Groups.Handlers
{
    public class UpdateGroupCommandHandler : IRequestHandler<UpdateGroupCommand, GroupDto>
    {
        readonly RepositoryFactory _groupRepoFactory;
        readonly IMapper _mapper;

        public UpdateGroupCommandHandler(RepositoryFactory groupRepoFactory, IMapper mapper)
        {
            _groupRepoFactory = groupRepoFactory;
            _mapper = mapper;
        }

        public async Task<GroupDto> Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
        {
            var groupRepo = await _groupRepoFactory.GetRepository(request.RealmId);

            var rec = _mapper.Map<Group>(request);
            rec.ModifiedBy = request.Author;
            rec.DateModified = rec.DateCreated;

            var res = await groupRepo.Update(rec);

            return _mapper.Map<GroupDto>(res);
        }
    }
}
