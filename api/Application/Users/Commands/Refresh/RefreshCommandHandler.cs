using Application.DTOs;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Users.Commands.Refresh
{
    public class RefreshCommandHandler : IRequestHandler<RefreshTokensCommand, Tokens>
    {
        private readonly ITokenService _tokenService;

        public RefreshCommandHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task<Tokens> Handle(RefreshTokensCommand request, CancellationToken cancellationtoken)
        {
            var tokens = await _tokenService.RefreshAccessToken(request.RefreshToken);

            if (tokens == null || string.IsNullOrEmpty(tokens.AccessToken))
            {
                return null;
            }

            return tokens;
        }
    }
}
