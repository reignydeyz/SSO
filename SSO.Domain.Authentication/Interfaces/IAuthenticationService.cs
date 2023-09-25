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

        /// <summary>
        /// Validates app then redirects to login page
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="applicationSecret"></param>
        /// <param name="callbackUrl"></param>
        /// <returns></returns>
        Task LoginRequest(string applicationId, string applicationSecret, string callbackUrl);

        /// <summary>
        /// Generates access token
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<string> GenerateAccessToken(Guid applicationId, Guid userId);

        /// <summary>
        /// Generates access token
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<string> GenerateAccessToken(Guid applicationId, string username);
    }
}
