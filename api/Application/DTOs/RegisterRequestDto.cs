using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public record RegisterRequestDto
    (
        string Name,
        string Email,
        string Password,
        string ConfirmPassword,
        string Role
    );
}
