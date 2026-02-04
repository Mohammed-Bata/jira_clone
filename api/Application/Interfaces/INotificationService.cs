using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface INotificationService
    {
        Task SendToUser (string UserId,NotificationDto notification);
    }
}
