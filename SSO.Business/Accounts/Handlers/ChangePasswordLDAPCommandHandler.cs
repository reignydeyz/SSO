using MediatR;
using SSO.Business.Accounts.Commands;

namespace SSO.Business.Accounts.Handlers
{
    public class ChangePasswordLdapCommandHandler : IRequestHandler<ChangePasswordLdapCommand, Unit>
    {
        public Task<Unit> Handle(ChangePasswordLdapCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
