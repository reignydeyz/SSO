using AutoMapper;
using MediatR;
using SSO.Business.ApplicationCallbacks.Commands;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;

namespace SSO.Business.ApplicationCallbacks.Handlers
{
    public class CreateAppCallbackCommandHandler : IRequestHandler<CreateAppCallbackCommand, AppCallbackDto>
    {
        readonly IApplicationCallbackRepository _applicationCallbackRepository;
        readonly IMapper _mapper;

        public CreateAppCallbackCommandHandler(IApplicationCallbackRepository applicationCallbackRepository, IMapper mapper)
        {
            _applicationCallbackRepository = applicationCallbackRepository;
            _mapper = mapper;
        }

        public async Task<AppCallbackDto> Handle(CreateAppCallbackCommand request, CancellationToken cancellationToken)
        {
            var rec = _mapper.Map<ApplicationCallback>(request);

            var res = await _applicationCallbackRepository.Add(rec);

            return _mapper.Map<AppCallbackDto>(res);
        }
    }
}
