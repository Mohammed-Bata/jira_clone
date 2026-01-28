using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.WorkItems.Commands.DeleteWorkItem
{
    public class DeleteWorkItemCommandHandler : IRequestHandler<DeleteWorkItemCommand>
    {
        private readonly IAppDbContext _context;
        public DeleteWorkItemCommandHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task Handle(DeleteWorkItemCommand request, CancellationToken cancellationToken)
        {
            var workItem = await _context.WorkItems.FindAsync(new object[] { request.WorkItemId }, cancellationToken);
            if (workItem == null)
            {
                throw new Exception("Work item not found");
            }
            _context.WorkItems.Remove(workItem);
            await _context.SaveChangesAsync(cancellationToken);
            return;
        }
    }
}
