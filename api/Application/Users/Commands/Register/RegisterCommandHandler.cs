using Application.DTOs;
using Application.Interfaces;
using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Users.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, User>
    {
        private readonly IAuthService _authService;
        public RegisterCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<User> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            bool isUnique = await _authService.IsEmailUniqueAsync(request.Email);

            if (!isUnique)
            {
                throw new Exception("email is exist");
            }

            var user = new User
            {
                
                Name = request.Name,
                Email = request.Email,
                
            };

            var result = await _authService.CreateUserAsync(user, request.Password);

            if (!result)
            {
                throw new Exception("create user failed");
            }

            
            return user;
        }
    }
}
