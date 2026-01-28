using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public record CreateWorkItemDto(
        string Title,
        string? Description,
        int ProjectColumnId,
        string? AssignedToUserId,
        Priority Priority = Priority.Medium,
        DateOnly? DueDate = null,
        ItemType Type = ItemType.Task
     );
    
}
