using Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Users.Commands.ExternalLogin
{
    public record ExternalLoginCommand(string Provider,string ProviderId,string Email,string Name,string Picture):IRequest<Tokens>;
    
}
