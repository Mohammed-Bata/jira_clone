using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Projects.Queries.GetProject
{
    public record GetProjectQuery(int ProjectId, string UserId) : IRequest<ProjectDto>;

    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ProjectColumnDto> Columns { get; set; }
    }

    public class ProjectColumnDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }
        public List<WorkItemDto> WorkItems { get; set; }
    }

    public class WorkItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }
        public string? AssignedToUserId { get; set; }
        public Priority Priority { get; set; }
        public DateOnly? DueDate { get; set; }
        public ItemType Type { get; set; }
    }
}
