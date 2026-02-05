using Application.Interfaces;
using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Projects.Commands.CreateProject
{
    public class CreateProjectCommandHandler:IRequestHandler<CreateProjectCommand, int>
    {
        private readonly IAppDbContext _context;

        public CreateProjectCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = new Project
            {
                Name = request.Name,
                Description = request.Description,
                OwnerId = request.OwnerId,
                Columns = new List<ProjectColumn>()
                {
                    new ProjectColumn
                    {
                        Order = 1,
                        Title = "To Do"
                    },
                    new ProjectColumn
                    {
                        Order = 2,
                        Title = "In Progress"
                    },
                    new ProjectColumn
                    {
                        Order = 3,
                        Title = "Done"
                    },
                },
                Members = new List<ProjectMember>
                {
                    new ProjectMember
                    {
                        UserId = request.OwnerId,
                        Role = ProjectRole.Owner
                    }
                }
            };



            _context.Projects.Add(project);
            await _context.SaveChangesAsync(cancellationToken);

            return project.Id;
        }   
      
    }
}
