using Application.Interfaces;
using MediatR;
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
            var project = await _context.Projects.FindAsync(new object[] { request.Id }, cancellationToken);
            if (project == null || project.OwnerId != request.UserId)
            {
                return false;
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
