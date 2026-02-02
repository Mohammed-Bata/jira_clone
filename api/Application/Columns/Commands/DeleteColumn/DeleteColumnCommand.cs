using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Columns.Commands.DeleteColumn
{
    public record DeleteColumnCommand(int ColumnId) : IRequest;
   
}
