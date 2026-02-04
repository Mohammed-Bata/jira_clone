using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Notifications.Queries.GetUnreadCount
{
    public record GetUnreadCountQuery(string UserId):IRequest<int>;
   
}
