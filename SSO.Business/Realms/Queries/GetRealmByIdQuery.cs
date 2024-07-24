using MediatR;

namespace SSO.Business.Realms.Queries
{
    public class GetRealmByIdQuery : IRequest<RealmDto>
    {
        public Guid RealmId { get; set; }
    }
}
