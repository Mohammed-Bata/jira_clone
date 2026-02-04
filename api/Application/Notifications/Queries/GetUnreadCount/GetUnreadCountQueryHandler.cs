using Application.Interfaces;
using Dapper;
using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Application.Notifications.Queries.GetUnreadCount
{
    public class GetUnreadCountQueryHandler:IRequestHandler<GetUnreadCountQuery,int>
    {
        private ISqlConnectionFactory _sqlConnectionFactory;

        public GetUnreadCountQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<int> Handle(GetUnreadCountQuery request,CancellationToken cancellationToken)
        {   
            using var connection = _sqlConnectionFactory.CreateConnection();

            var sql = @"SELECT COUNT(*) FROM Notifications WHERE UserId = @UserId AND IsRead = 0";

            return await connection.ExecuteScalarAsync<int>(new CommandDefinition(
            sql,
            new { UserId = request.UserId },
            cancellationToken: cancellationToken
            ));
        }
    }
}
