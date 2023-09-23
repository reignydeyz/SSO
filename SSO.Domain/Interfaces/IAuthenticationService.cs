﻿namespace SSO.Domain.Interfaces
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
