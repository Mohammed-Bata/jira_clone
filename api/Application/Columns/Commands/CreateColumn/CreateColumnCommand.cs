using Application.Projects.Queries.GetProject;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Columns.Commands.CreateColumn
{
    public record CreateColumnCommand(
        string Title,
        int ProjectId
     ):IRequest<CreateColumnResult>;

    public record CreateColumnResult(int Id,
        double Order,
        string Title,
        List<WorkItemDto> WorkItems);
   
}
