using AutoMapper;
using MediatR;
using SSO.Business.Realms.Queries;
using SSO.Domain.Management.Interfaces;

namespace SSO.Business.Realms.Handlers
{
    public class GetRealmByIdQueryHandler : IRequestHandler<GetRealmByIdQuery, RealmDto>
    {
        readonly IRealmRepository _realmRepository;
        readonly IMapper _mapper;

        public GetRealmByIdQueryHandler(IRealmRepository realmRepository, IMapper mapper)
        {
            _realmRepository = realmRepository;
            _mapper = mapper;
        }

        public async Task<RealmDto> Handle(GetRealmByIdQuery request, CancellationToken cancellationToken)
        {
            var rec = await _realmRepository.FindOne(x => x.RealmId == request.RealmId);

            var res = _mapper.Map<RealmDto>(rec);

            return res;
        }
    }
}
