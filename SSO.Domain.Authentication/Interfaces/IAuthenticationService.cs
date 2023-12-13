using SSO.Domain.Models;

namespace SSO.Domain.Authentication.Interfaces
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// Validate credentials
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="app"></param>
        Task Login(string username, string password, Application app);
    }
}
