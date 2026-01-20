using Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Users.Commands.Login
{
    public record LoginCommand(string Email,string Password):IRequest<Tokens>;
    
}
