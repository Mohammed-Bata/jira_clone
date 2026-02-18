using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Hubs
{
    [Authorize]
    public class NotificationHub:Hub
    {
        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;

            await base.OnConnectedAsync();
        }

        public async Task JoinProject(string projectId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Project_{projectId}");
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.UserIdentifier;

            Console.WriteLine($"❌ User {userId} disconnected from NotificationHub");

            await base.OnDisconnectedAsync(exception);
        }

        public async Task LeaveProject(string projectId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Project_{projectId}");
        }
    }
}
