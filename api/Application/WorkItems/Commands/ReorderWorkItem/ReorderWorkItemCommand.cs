using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.WorkItems.Commands.ReorderWorkItem
{
    public record ReorderWorkItemCommand(
        int WorkItemId,
        double? PrevOrder,
        double? NextOrder
     ):IRequest<double>;
   
}
