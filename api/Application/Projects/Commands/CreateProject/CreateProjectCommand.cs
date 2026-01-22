using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Projects.Commands.CreateProject
{
    public record CreateProjectCommand(string Name, string Description, string OwnerId):IRequest<int>;
   
}
