using MediatR;

namespace SSO.Business.Groups.Queries
{
    public class GetGroupsQuery : IRequest<IQueryable<GroupDto>>
    {
    }
}
