using AutoMapper;
using MediatR;
using SSO.Business.Groups.Commands;
using SSO.Domain.Models;

namespace SSO.Business.Groups.Handlers
{
    public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, GroupDto>
    {
        readonly RepositoryFactory _groupRepoFactory;
        readonly IMapper _mapper;

        public CreateGroupCommandHandler(RepositoryFactory groupRepoFactory, IMapper mapper)
        {
            _groupRepoFactory = groupRepoFactory;
            _mapper = mapper;
        }

        public async Task<GroupDto> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
        {
            var groupRepo = await _groupRepoFactory.GetRepository(request.RealmId);

            var rec = _mapper.Map<Group>(request);
            rec.CreatedBy = request.Author;
            rec.DateCreated = DateTime.Now;
            rec.ModifiedBy = request.Author;
            rec.DateModified = rec.DateCreated;

            var res = await groupRepo.Add(rec);

            return _mapper.Map<GroupDto>(res);
        }
    }
}
