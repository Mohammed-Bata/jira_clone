using Application.DTOs;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Application.WorkItems.Commands.CreateWorkItem
{
    public class CreateWorkItemCommandHandler: IRequestHandler<CreateWorkItemCommand, CreateWorkItemResult>
    {
        private readonly IAppDbContext _context;
        private readonly INotificationService _notificationService;

        public CreateWorkItemCommandHandler(IAppDbContext context,INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
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


            var notification = new Notification
            {
                UserId = request.AssignedToUserId,
                Message = "Assigned Workitem To You",
                ActorId = request.AuthorUserId,
            };

            _context.Notifications.Add(notification);


            await _context.SaveChangesAsync(cancellationToken);


            var notificationDto = new NotificationDto
            {
                Id = notification.Id,
                Message = notification.Message,
                ActorId = notification.ActorId,
                ActorName = request.AuthorName,
                CreatedAt = notification.CreatedAt,
                IsRead = notification.IsRead,
            };

            await _notificationService.SendToUser(request.AssignedToUserId,notificationDto);

            var result = new CreateWorkItemResult
            (
                workItem.Id,
                workItem.Title,
                workItem.AssignedToUserId,
                request.AssignedToUserName,
                workItem.Order,
                workItem.Priority,
                workItem.DueDate,
                workItem.Type
            );
 
            return result;
        }
        
    }
}
