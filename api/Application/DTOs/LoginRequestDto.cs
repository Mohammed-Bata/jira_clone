using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public record LoginRequestDto(
        string Email,
        string Password
    );
    
}
