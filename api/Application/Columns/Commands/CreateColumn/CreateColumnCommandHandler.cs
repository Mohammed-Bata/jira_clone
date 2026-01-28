using Application.Interfaces;
using Application.Projects.Queries.GetProject;
using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Columns.Commands.CreateColumn
{
    public class CreateColumnCommandHandler: IRequestHandler<CreateColumnCommand, CreateColumnResult>
    {
        private readonly IAppDbContext _context;

        public CreateColumnCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<CreateColumnResult> Handle(CreateColumnCommand request, CancellationToken cancellationToken)
        {
            var maxOrder = _context.ProjectColumns
                .Where(c => c.ProjectId == request.ProjectId)
                .Select(c => (double?)c.Order)
                .Max() ?? 0;

            var column = new ProjectColumn
            {
                Order = maxOrder + 1,
                Title = request.Title,
                ProjectId = request.ProjectId
            };
            _context.ProjectColumns.Add(column);
            await _context.SaveChangesAsync(cancellationToken);
            return new CreateColumnResult(
                column.Id,
                column.Order,
                column.Title,
                new List<WorkItemDto>()
            );
        }
    }
}
