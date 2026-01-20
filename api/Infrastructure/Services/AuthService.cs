using Application.Interfaces;
using Domain;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Services
{
    public class AuthService : IAuthService
    {

        private readonly UserManager<AppUser> _userManager;
    
        public AuthService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<User?> CheckPasswordAsync(string email, string password)
        {
            var applicationUser = await _userManager.FindByEmailAsync(email);
            if (applicationUser == null)
            {
                return null;
            }
            var result = await _userManager.CheckPasswordAsync(applicationUser, password);

            if (result == false)
            {
                return null;
            }
            return new User
            {
                Id = applicationUser.Id,
                Name = applicationUser.Name,
                Email = applicationUser.Email
            };
        }

        public async Task<bool> CreateUserAsync(User user, string password)
        {
            var applicationUser = new AppUser
            {
                Name = user.Name,
                Email = user.Email
            };

            var result = await _userManager.CreateAsync(applicationUser, password);

            if (!result.Succeeded)
            {
                return false;
            }
            

            return true;
        }

        public async Task<User?> FindByEmailAsync(string email)
        {
            var applicationUser = await _userManager.FindByEmailAsync(email);
            if (applicationUser == null)
            {
                return null;
            }
            var user = new User
            {
                Id = applicationUser.Id,
                Name = applicationUser.Name,
                Email = applicationUser.Email
            };
            return user;
        }

        public async Task<string> FindEmailByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            return user.Email;
        }

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email) == null;
        }

        //public async Task<User?> FindByProviderAsync(string provider, string providerId)
        //{
        //    var userLoginInfo = new UserLoginInfo(provider, providerId, provider);
        //    var applicationUser = await _userManager.FindByLoginAsync(userLoginInfo.LoginProvider, userLoginInfo.ProviderKey);
        //    if (applicationUser == null)
        //    {
        //        return null;
        //    }
        //    return new User
        //    {
        //        Id = applicationUser.Id,
        //        Name = applicationUser.Name,
        //        Email = applicationUser.Email
        //    };

        //}

       public async Task<User?> CreateOrUpdateUserWithProviderAsync(string email, string name, string provider, string providerId,string picture)
        {
            var userLoginInfo = new UserLoginInfo(provider, providerId, provider);
            var applicationUser = await _userManager.FindByEmailAsync(email);
            if (applicationUser == null)
            {
                applicationUser = new AppUser
                {
                    UserName = email,
                    Email = email,
                    Name = name,
                    AvatarUrl = picture
                };
                var createResult = await _userManager.CreateAsync(applicationUser);
                if (!createResult.Succeeded)
                {
                   return null;
                }
            }

            var logins = await _userManager.GetLoginsAsync(applicationUser);
            if (!logins.Any(l => l.LoginProvider == provider && l.ProviderKey == providerId))
            {
                var addLoginResult = await _userManager.AddLoginAsync(applicationUser, userLoginInfo);
                if (!addLoginResult.Succeeded)
                {
                    var user = new User
                    {
                        Id = applicationUser.Id,
                        Name = applicationUser.Name,
                        Email = applicationUser.Email,
                        AvatarUrl = applicationUser.AvatarUrl,
                    };
                    return user;
                }
            }



            return new User
            {
                Id = applicationUser.Id,
                Name = applicationUser.Name,
                Email = applicationUser.Email,
                AvatarUrl = applicationUser.AvatarUrl,
            };
        }

    }
}
