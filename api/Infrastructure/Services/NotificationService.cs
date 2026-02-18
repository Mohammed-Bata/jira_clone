using Application.DTOs;
using Application.Interfaces;
using Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
       
        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendToUser(string UserId, NotificationDto notification)
        {
            await _hubContext.Clients.User(UserId).SendAsync("ReceiveNotification", notification); 
        }

        public async Task SendToProject(string ProjectId, NotificationDto notification)
        {
            await _hubContext.Clients.Group($"Project_{ProjectId}").SendAsync("ReceiveNotification", notification);
        }
    }
}
