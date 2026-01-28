using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.WorkItems.Commands.DeleteWorkItem
{
    public record DeleteWorkItemCommand(int WorkItemId): IRequest;

}
