using Application.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Users.Commands.Register
{
    public class RegisterCommandValidator:AbstractValidator<RegisterCommand>
    {
        private readonly IAuthService _authService;

        public RegisterCommandValidator(IAuthService authService)
        {
            _authService = authService;
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MustAsync(BeUniqueEmail).WithMessage("This email is already in use.");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");            
        }

        private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
        {
            // Returns true if NO user exists with this email
            // We use ToLower() to ensure "User@me.com" and "user@me.com" are treated as the same
            return await _authService.IsEmailUniqueAsync(email);
        }


    }
}
