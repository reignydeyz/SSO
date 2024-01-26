﻿using SSO.Domain.Models;
using System.Security.Claims;

namespace SSO.Domain.Management.Interfaces
{
    public interface IUserClaimRepository
    {
        /// <summary>
        /// Gets user claims
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        Task<IEnumerable<Claim>> GetClaims(string userName, Guid applicationId);

        /// <summary>
        /// Gets user claims
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        Task<IEnumerable<Claim>> GetClaims(Guid userId, Guid applicationId);

        Task AddClaims(Guid userId, IEnumerable<ApplicationPermission> claims, bool? saveChanges = true);

        Task AddClaims(IEnumerable<ApplicationUser> users, IEnumerable<ApplicationPermission> claims, bool? saveChanges = true);

        Task RemoveClaims(Guid userId, IEnumerable<ApplicationPermission> claims, bool? saveChanges = true);

        Task RemoveClaims(IEnumerable<ApplicationUser> users, IEnumerable<ApplicationPermission> claims, bool? saveChanges = true);
    }
}
