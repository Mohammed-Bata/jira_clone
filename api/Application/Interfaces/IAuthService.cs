using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<User?> FindByEmailAsync(string email);
        Task<string> FindEmailByIdAsync(string userId);
        Task<User?> CheckPasswordAsync(string email, string password);
        Task<bool> IsEmailUniqueAsync(string email);
        Task<bool> CreateUserAsync(User user, string password);
        //Task<User?> FindByProviderAsync(string provider, string providerId);
        Task<User?> CreateOrUpdateUserWithProviderAsync(string email,string name, string provider, string providerId,string picture);
    }
}
