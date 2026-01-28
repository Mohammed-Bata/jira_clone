using Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Users.Commands.Refresh
{
    public record RefreshTokensCommand(string RefreshToken) : IRequest<Tokens>;
}
