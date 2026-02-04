using Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Notifications.Queries.GetNotificationsByStatus
{
    public record GetNotificationsByStatusQuery(string UserId,string Status,int Page,int PageSize):IRequest<List<NotificationDto>>;
    
}
