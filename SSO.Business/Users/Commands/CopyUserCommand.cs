using MediatR;

namespace SSO.Business.Users.Commands
{
    public class CopyUserCommand : UpdateUserCommand, IRequest<UserDto>
    {
    }
}
