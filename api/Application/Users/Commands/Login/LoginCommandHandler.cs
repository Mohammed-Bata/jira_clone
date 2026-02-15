using Application.DTOs;
using Application.Interfaces;
using Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Users.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Tokens>
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;
            public LoginCommandHandler(IAuthService authService, ITokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
        }

        public async Task<Tokens> Handle(LoginCommand request, CancellationToken cancellationToken)
        {

            var Result = await _authService.CheckPasswordAsync(request.Email, request.Password);

            if (!Result.IsSuccess)
            {
                throw new BadRequestException(Result.Error);
            }

            var jwtTokenId = Guid.NewGuid().ToString();
            var accessToken = await _tokenService.GetAccessToken(Result.Value, jwtTokenId);
            var refreshToken = await _tokenService.CreateNewRefreshToken(Result.Value.Id, jwtTokenId);

            return new Tokens
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}
