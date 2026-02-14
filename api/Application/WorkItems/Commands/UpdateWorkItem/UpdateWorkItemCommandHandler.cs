using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.WorkItems.Commands.UpdateWorkItem
{
    public class UpdateWorkItemCommandHandler:IRequestHandler<UpdateWorkItemCommand,bool>
    {
        private IAppDbContext _context;

        public UpdateWorkItemCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateWorkItemCommand request,CancellationToken cancellationToken)
        {
            var workItem = await _context.WorkItems.FirstOrDefaultAsync(w => w.Id == request.Id);

            if (workItem == null)
            {
                return false;
            }

            if(request.dto.Title != null)
            {
                workItem.Title = request.dto.Title;
            }

            if(request.dto.Description != null)
            {
                workItem.Description = request.dto.Description;
            }
            if (request.dto.AssignedToUserId != null)
            {
                workItem.AssignedToUserId = request.dto.AssignedToUserId;
            }
            if (request.dto.Type != null)
            {
                workItem.Type = request.dto.Type.Value;
            }
            if(request.dto.Priority != null)
            {
                workItem.Priority = request.dto.Priority.Value;
            }
            if(request.dto.DueDate != null)
            {
                workItem.DueDate = DateOnly.FromDateTime((DateTime)request.dto.DueDate);
            }

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
