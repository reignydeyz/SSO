using MediatR;

namespace SSO.Business.Groups.Queries
{
    public class GetGroupByIdQuery : IRequest<GroupDto>
    {
        public Guid GroupId { get; set; }
    }
}
