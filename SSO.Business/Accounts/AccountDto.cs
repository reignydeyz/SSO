using SSO.Business.Applications;
using SSO.Business.Users;

namespace SSO.Business.Accounts
{
    public class AccountDto : UserDetailDto
    {
        public IList<ApplicationDto> Apps { get; set; }
    }
}
