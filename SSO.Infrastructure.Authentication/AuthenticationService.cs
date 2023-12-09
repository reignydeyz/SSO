﻿using Microsoft.AspNetCore.Identity;
using SSO.Domain.Authentication.Interfaces;
using SSO.Domain.Models;

namespace SSO.Infrastructure.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthenticationService(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task Login(string username, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(username, password, false, false);

            if (!result.Succeeded)
                throw new UnauthorizedAccessException();
        }
    }
}
