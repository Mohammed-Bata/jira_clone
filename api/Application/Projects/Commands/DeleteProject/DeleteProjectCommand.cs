using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Projects.Commands.DeleteProject
{
    public record DeleteProjectCommand(int Id, string UserId) : IRequest<bool>;
    
}
