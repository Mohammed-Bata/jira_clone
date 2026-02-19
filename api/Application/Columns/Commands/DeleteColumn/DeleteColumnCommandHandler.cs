using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Columns.Commands.DeleteColumn
{
    public class DeleteColumnCommandHandler: IRequestHandler<DeleteColumnCommand>
    {
        private readonly IAppDbContext _context;

        public DeleteColumnCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteColumnCommand request, CancellationToken cancellationToken)
        {

            var maxOrder = await _context.WorkItems
            .Where(w => w.ProjectColumnId == request.TargetColumnId)
            .MaxAsync(w => (double?)w.Order, cancellationToken) ?? 0;

            if (request.WorkItemIds!= null && request.WorkItemIds.Any())
            {
                await _context.WorkItems
                    .Where(w => request.WorkItemIds.Contains(w.Id))
                    .ExecuteUpdateAsync(s => s
                        .SetProperty(w => w.ProjectColumnId, request.TargetColumnId)
                        .SetProperty(w => w.Order, w => w.Order + maxOrder + 100),cancellationToken);
            }

            var column =  await _context.ProjectColumns.FindAsync(new object[] { request.ColumnId }, cancellationToken);
            if(column == null)
            {
                throw new Exception("Column not found");
            }
            _context.ProjectColumns.Remove(column);
            await _context.SaveChangesAsync(cancellationToken);
            return;
        }
    }
}
