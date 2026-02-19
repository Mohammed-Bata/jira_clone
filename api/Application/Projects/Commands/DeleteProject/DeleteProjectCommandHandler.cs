using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Projects.Commands.DeleteProject
{
    public class DeleteProjectCommandHandler:IRequestHandler<DeleteProjectCommand, bool>
    {
        private readonly IAppDbContext _context;

        public DeleteProjectCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _context.Projects
                .Include(p => p.Columns)
                    .ThenInclude(c => c.WorkItems)
                .FirstOrDefaultAsync(p => p.Id == request.Id);
           
            if (project == null || project.OwnerId != request.UserId)
            {
                return false;
            }
            var allWorkItems = project.Columns.SelectMany(c => c.WorkItems);
            _context.WorkItems.RemoveRange(allWorkItems);

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
