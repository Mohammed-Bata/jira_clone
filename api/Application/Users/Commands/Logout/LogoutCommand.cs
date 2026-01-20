using Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Users.Commands.Logout
{
    public record LogoutCommand(Tokens tokens) : IRequest<bool>;
}
