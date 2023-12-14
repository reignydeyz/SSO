using AutoMapper;
using MediatR;
using SSO.Business.Applications.Commands;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;

namespace SSO.Business.Applications.Handlers
{
    public class CreateAppCommandHandler : IRequestHandler<CreateAppCommand, ApplicationDto>
    {
        readonly IApplicationRepository _applicationRepository;
        readonly IMapper _mapper;

        public CreateAppCommandHandler(IApplicationRepository applicationRepository, IMapper mapper)
        {
            _applicationRepository = applicationRepository;
            _mapper = mapper;
        }

        public async Task<ApplicationDto> Handle(CreateAppCommand request, CancellationToken cancellationToken)
        {
            var rec = _mapper.Map<Application>(request);
            rec.CreatedBy = request.Author;
            rec.DateCreated = DateTime.Now;
            rec.ModifiedBy = request.Author;
            rec.DateModified = rec.DateCreated;

            var res = await _applicationRepository.Add(rec);

            return _mapper.Map<ApplicationDto>(res);
        }
    }
}
