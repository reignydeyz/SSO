using AutoMapper;
using MediatR;
using SSO.Business.Applications.Commands;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;

namespace SSO.Business.Applications.Handlers
{
    public class UpdateApplicationCommandHandler : IRequestHandler<UpdateAppCommand, ApplicationDetailDto>
    {
        readonly IApplicationRepository _applicationRepository;
        readonly IMapper _mapper;

        public UpdateApplicationCommandHandler(IApplicationRepository applicationRepository, IMapper mapper)
        {
            _applicationRepository = applicationRepository;
            _mapper = mapper;
        }

        public async Task<ApplicationDetailDto> Handle(UpdateAppCommand request, CancellationToken cancellationToken)
        {
            var rec = _mapper.Map<Application>(request);
            rec.ModifiedBy = request.Author;
            rec.DateModified = rec.DateCreated;

            var res = await _applicationRepository.Update(rec);

            return _mapper.Map<ApplicationDetailDto>(res);
        }
    }
}
