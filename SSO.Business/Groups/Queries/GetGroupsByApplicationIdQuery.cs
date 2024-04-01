using MediatR;

namespace SSO.Business.Groups.Queries
{
    public class GetGroupsByApplicationIdQuery : IRequest<IQueryable<GroupDto>>
    {
        public Guid ApplicationId { get; set; }
    }
}
