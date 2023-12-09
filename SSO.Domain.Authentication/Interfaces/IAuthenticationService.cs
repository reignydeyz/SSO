namespace SSO.Domain.Authentication.Interfaces
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// Validate credentials
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        Task Login(string username, string password);
    }
}
