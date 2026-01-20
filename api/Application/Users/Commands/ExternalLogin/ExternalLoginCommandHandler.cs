using Application.DTOs;
using Application.Interfaces;
using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Users.Commands.ExternalLogin
{
    public class ExternalLoginCommandHandler:IRequestHandler<ExternalLoginCommand,Tokens>
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;

        public ExternalLoginCommandHandler(IAuthService authService, ITokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
        }

        public async Task<Tokens> Handle (ExternalLoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _authService.CreateOrUpdateUserWithProviderAsync(
                request.Email,
                request.Name,
                request.Provider,
                request.ProviderId,
                request.Picture
            );

            var jwtTokenId = Guid.NewGuid().ToString();

            var accessToken = await _tokenService.GetAccessToken(user,jwtTokenId);
            var refreshToken = await _tokenService.CreateNewRefreshToken(user.Id, jwtTokenId);

            return new Tokens
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

        }
    }
}
