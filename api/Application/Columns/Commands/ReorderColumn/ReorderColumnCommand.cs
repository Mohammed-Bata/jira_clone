using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Columns.Commands.ReorderColumn
{
    public record ReorderColumnCommand(
        int ColumnId,
        double? PrevOrder,
        double? NextOrder
     ):IRequest<double>;
    
}
