using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.WorkItems.Commands.CreateWorkItem
{
    public record CreateWorkItemCommand(string AuthorName,
                                        string Title,
                                         string? Description,
                                         int ProjectColumnId,
                                         string? AssignedToUserId,
                                         string? AssignedToUserName,
                                         string AuthorUserId,
                                         Priority Priority = Priority.Medium,
                                         DateOnly? DueDate = null,
                                         ItemType Type = ItemType.Task):IRequest<CreateWorkItemResult>;

    public record CreateWorkItemResult(int Id,string Title,
                                         string? AssignedToUserId,
                                         string? AssignedToUserName,
                                         double Order,
                                         Priority Priority = Priority.Medium,
                                         DateOnly? DueDate = null,
                                         ItemType Type = ItemType.Task);

}
