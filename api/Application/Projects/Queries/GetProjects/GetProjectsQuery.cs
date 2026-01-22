using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Projects.Queries.GetProjects
{
    public record GetProjectsQuery(string UserId):IRequest<List<ProjectsDto>>;

    public class ProjectsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
