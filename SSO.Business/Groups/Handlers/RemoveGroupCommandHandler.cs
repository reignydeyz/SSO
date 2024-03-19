using MediatR;
using SSO.Business.Groups.Commands;
using SSO.Domain.Management.Interfaces;

namespace SSO.Business.Groups.Handlers
{
    public class RemoveGroupCommandHandler : IRequestHandler<RemoveGroupCommand, Unit>
    {
        readonly IGroupRepository _groupRepository;

        public RemoveGroupCommandHandler(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task<Unit> Handle(RemoveGroupCommand request, CancellationToken cancellationToken)
        {
            var rec = await _groupRepository.FindOne(x => x.GroupId == request.GroupId);

            if (rec is null)
                throw new ArgumentNullException();

            await _groupRepository.Delete(rec);

            return new Unit();
        }
    }
}
