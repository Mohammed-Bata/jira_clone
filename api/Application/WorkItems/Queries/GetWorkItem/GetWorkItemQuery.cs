using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.WorkItems.Queries.GetWorkItem
{
    public record GetWorkItemQuery(int Id):IRequest<WorkItemDto>;
    

    public record WorkItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? AssignedToUserId { get; set; }
        public string? AssignedToUserName { get; set; }
        public string AuthorUserId { get; set; }
        public string AuthorUserName { get; set; }
        public Priority Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public ItemType Type { get; set; }
    }
}
