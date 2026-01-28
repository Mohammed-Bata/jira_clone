using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.WorkItems.Commands.CreateWorkItem
{
    public class CreateWorkItemCommandHandler: IRequestHandler<CreateWorkItemCommand, CreateWorkItemResult>
    {
        private readonly IAppDbContext _context;

        public CreateWorkItemCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<CreateWorkItemResult> Handle(CreateWorkItemCommand request, CancellationToken cancellationToken)
        {
            var maxOrder = await _context.WorkItems
                            .Where(w => w.ProjectColumnId == request.ProjectColumnId)
                            .MaxAsync(w => (double?)w.Order) ?? 0;

            var workItem = new WorkItem
            {
                Title = request.Title,
                Description = request.Description,
                ProjectColumnId = request.ProjectColumnId,
                AssignedToUserId = request.AssignedToUserId,
                AuthorUserId = request.AuthorUserId,
                Order = maxOrder + 100,
                Priority = request.Priority,
                DueDate = request.DueDate,
                Type = request.Type
            };

            _context.WorkItems.Add(workItem);
            await _context.SaveChangesAsync(cancellationToken);

            var result = new CreateWorkItemResult
            (
                workItem.Id,
                workItem.Title,
                workItem.AssignedToUserId,
                workItem.Order,
                workItem.Priority,
                workItem.DueDate,
                workItem.Type
            );
 
            return result;
        }
        
    }
}
