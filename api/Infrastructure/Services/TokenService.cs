using Application.DTOs;
using Application.Interfaces;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services
{
    public class TokenService: ITokenService
    {
        private readonly UserManager<AppUser> _userManager;
        private string secretKey;
        private readonly AppDbContext _context;

        public TokenService(UserManager<AppUser> userManager, IConfiguration configuration, AppDbContext context)
        {
            _userManager = userManager;
            secretKey = configuration["ApiSettings:Secret"];
            _context = context;
        }

        public async Task<string> GetAccessToken(User user, string jwtTokenId)
        {
            var applicationUser = await _userManager.FindByIdAsync(user.Id);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]{
                    new Claim(JwtRegisteredClaimNames.Sub, applicationUser.Id),
                    new Claim(ClaimTypes.Name, applicationUser.Name),
                    new Claim(ClaimTypes.Email, applicationUser.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, jwtTokenId),
                }),
                //Issuer = "TechHubAPI",
                //Audience = "TechHubClient",
                Expires = DateTime.Now.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = new JwtSecurityTokenHandler().CreateToken(tokenDescriptor);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public async Task<Tokens> RefreshAccessToken(string refreshToken)
        {
            var existingRefreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Refresh_Token == refreshToken);
            if (existingRefreshToken == null || !existingRefreshToken.IsValid)
            {
                return new Tokens();
            }

            if (!existingRefreshToken.IsValid)
            {
                await MarkAllTokenInChainAsInvalid(existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);
            }

            if (existingRefreshToken.ExpiresAt < DateTime.UtcNow)
            {
                await MarkTokenAsInvalid(existingRefreshToken);
                return new Tokens();
            }

            var newRefreshToken = await CreateNewRefreshToken(existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);

            await MarkTokenAsInvalid(existingRefreshToken);

            var appuser = await _userManager.FindByIdAsync(existingRefreshToken.UserId.ToString());
            if (appuser == null)
            {
                return new Tokens();
            }
            var user = new User()
            {
                Id = appuser.Id,
                Name = appuser.Name,
                Email = appuser.Email,
            };

            var newAccessToken = await GetAccessToken(user, existingRefreshToken.JwtTokenId);
            return new Tokens()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }

        private async Task MarkTokenAsInvalid(RefreshToken refreshToken)
        {
            refreshToken.IsValid = false;
            _context.RefreshTokens.Update(refreshToken);
            await _context.SaveChangesAsync();
        }

        public async Task<string> CreateNewRefreshToken(string userId, string jwtTokenId)
        {
            await MarkAllTokenInChainAsInvalid(userId, jwtTokenId);
            RefreshToken refreshToken = new RefreshToken()
            {
                UserId = userId,
                JwtTokenId = jwtTokenId,
                IsValid = true,
                ExpiresAt = DateTime.Now.AddDays(10),
                Refresh_Token = Guid.NewGuid() + "-" + Guid.NewGuid(),
            };
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
            return refreshToken.Refresh_Token;
        }

        private async Task MarkAllTokenInChainAsInvalid(string userId, string jwtTokenId)
        {
            await _context.RefreshTokens.Where(u => u.UserId == userId
              && u.JwtTokenId == jwtTokenId)
                  .ExecuteUpdateAsync(u => u.SetProperty(refreshToken => refreshToken.IsValid, false));
        }

        

        public async Task<bool> RevokeRefreshToken(string refreshToken)
        {
            var existingRefreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Refresh_Token == refreshToken);
            if (existingRefreshToken == null)
            {
                return false;
            }

            await MarkAllTokenInChainAsInvalid(existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);

            return true;
        }
    }
}
