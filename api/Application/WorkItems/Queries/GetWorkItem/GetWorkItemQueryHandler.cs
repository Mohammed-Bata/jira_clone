using Application.Interfaces;
using Dapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.WorkItems.Queries.GetWorkItem
{
    public class GetWorkItemQueryHandler : IRequestHandler<GetWorkItemQuery, WorkItemDto>
    {
        private ISqlConnectionFactory _sqlConnectionFactory;

        public GetWorkItemQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<WorkItemDto> Handle(GetWorkItemQuery request, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();
            var sql = @"SELECT i.Id,i.Title,i.Description,i.AssignedToUserId,u.Name AS AssignedToUserName,i.AuthorUserId,au.Name AS AuthorUserName,i.Priority,i.DueDate,i.Type FROM WorkItems i 
                        LEFT JOIN dbo.AspNetUsers u ON i.AssignedToUserId = u.Id
                        JOIN dbo.AspNetUsers au ON i.AuthorUserId = au.Id
                        WHERE i.Id = @Id;";

            var workItem = await connection.QuerySingleOrDefaultAsync<WorkItemDto>(sql, new { Id = request.Id });

            return workItem;
        }
    }
}
