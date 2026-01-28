using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.WorkItems.Commands.ReorderWorkItem
{
    public record ReorderWorkItemCommand(
        int WorkItemId,
        int ColumnId,
        double? PrevOrder,
        double? NextOrder
     ):IRequest<double>;
   
}
