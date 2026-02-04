using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Notifications.Commands.MarkAllRead
{
    public record MarkAllNotificationsReadCommand(string UserId):IRequest<bool>;
    
}
