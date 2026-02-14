using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public class UpdateWorkItemDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? AssignedToUserId { get; set; }
        public Priority? Priority { get; set; }
        public ItemType? Type { get; set; }
        public DateTime? DueDate { get; set; }

    }
}
