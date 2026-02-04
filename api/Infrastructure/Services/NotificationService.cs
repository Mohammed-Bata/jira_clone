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
        private readonly AppDbContext _context;

        public NotificationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task SendToUser(string UserId, NotificationDto notification)
        {
            await _hubContext.Clients.User(UserId).SendAsync("ReceiveNotification", notification); 
        }
    }
}
