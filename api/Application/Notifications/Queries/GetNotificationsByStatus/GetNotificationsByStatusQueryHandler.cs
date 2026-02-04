using Application.DTOs;
using Application.Interfaces;
using Dapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace Application.Notifications.Queries.GetNotificationsByStatus
{
    public class GetNotificationsByStatusQueryHandler:IRequestHandler<GetNotificationsByStatusQuery,List<NotificationDto>>
    {
        private ISqlConnectionFactory _connectionFactory;

        public GetNotificationsByStatusQueryHandler(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<NotificationDto>> Handle (GetNotificationsByStatusQuery request, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.CreateConnection();

            bool? isRead = request.Status switch
            {
                "unread" => false,
                "read" => true,
                "all" => null,
                _ => null
            };

            var sql = @"SELECT n.Id,n.Message,n.CreatedAt,n.IsRead,u.Name FROM Notifications n LEFT JOIN Users u ON n.ActorId = u.Id
            WHERE n.UserId = @userId AND (@IsRead IS NULL OR n.IsRead = @IsRead)
            ORDER BY n.CreatedAt DESC
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";
            
            var Notifications = await connection.QueryAsync<NotificationDto>(
                new CommandDefinition(sql,
                new {userId = request.UserId,
                    IsRead = isRead,
                    Offset = (request.Page - 1) * request.PageSize,
                    PageSize = request.PageSize,
                } ,
                cancellationToken: cancellationToken));

            return Notifications.ToList();
        }
    }
}
