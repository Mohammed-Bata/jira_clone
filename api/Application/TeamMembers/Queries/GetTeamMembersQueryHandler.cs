using Application.DTOs;
using Application.Interfaces;
using Dapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.TeamMembers.Queries
{
    public class GetTeamMembersQueryHandler:IRequestHandler<GetTeamMembersQuery, List<UserDto>>
    {
        private ISqlConnectionFactory _sqlConnectionFactory;

        public GetTeamMembersQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<List<UserDto>> Handle (GetTeamMembersQuery request, CancellationToken cancellationToken)
        {
                       using var connection = _sqlConnectionFactory.CreateConnection();
            var sql = @"SELECT u.Id,u.Name,u.Email FROM ProjectMembers pm
                        JOIN dbo.AspNetUsers u ON pm.UserId = u.Id
                        WHERE pm.ProjectId = @ProjectId;";
            var result = await connection.QueryAsync<UserDto>(sql, new { ProjectId = request.ProjectId });
            return result.ToList();
        }
    }
}
