using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Application.DTOs
{
    public record CreateColumnDto(
        string Title,
        int ProjectId
     );
    
}
