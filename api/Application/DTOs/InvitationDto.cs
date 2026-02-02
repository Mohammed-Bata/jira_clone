using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public record InvitationDto(
        string Email,
        int ProjectId
    );
    
}
