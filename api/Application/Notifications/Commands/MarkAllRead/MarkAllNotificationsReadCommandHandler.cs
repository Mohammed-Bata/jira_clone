using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Notifications.Commands.MarkAllRead
{
    public class MarkAllNotificationsReadCommandHandler:IRequestHandler<MarkAllNotificationsReadCommand,bool>
    {
        private readonly IAppDbContext _context;

        public MarkAllNotificationsReadCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(MarkAllNotificationsReadCommand request,CancellationToken cancellationToken)
        {
            var updatedRows = await _context.Notifications.Where(n => n.UserId == request.UserId && !n.IsRead).ExecuteUpdateAsync(n => n.SetProperty(x => x.IsRead, true), cancellationToken);

            return updatedRows > 0 ? true : false;
        }
    }
}
