using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Users.Commands.Register
{
    public record RegisterCommand(string Name,string Email,string Password,string ConfirmPassword) :IRequest<User>;
    
}
