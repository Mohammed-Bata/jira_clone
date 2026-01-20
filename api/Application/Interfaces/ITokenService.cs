using Application.DTOs;
using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface ITokenService
    {
        Task<string> GetAccessToken(User user, string jwtTokenId);
        Task<string> CreateNewRefreshToken(string userId, string jwtTokenId);
        Task<Tokens> RefreshAccessToken(string refreshToken);
        Task<bool> RevokeRefreshToken(string refreshToken);
    }
}
