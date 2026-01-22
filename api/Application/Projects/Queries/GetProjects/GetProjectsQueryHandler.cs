using Application.Interfaces;
using Application.Projects.Queries.GetProject;
using Dapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Projects.Queries.GetProjects
{
    public class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, List<ProjectsDto>>
    {
        private ISqlConnectionFactory _sqlConnectionFactory;

        public GetProjectsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<List<ProjectsDto>> Handle (GetProjectsQuery request, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();
            var sql = "SELECT Id, Name FROM Projects WHERE OwnerId = @UserId";
            var projects = await connection.QueryAsync<ProjectsDto>(sql, new { UserId = request.UserId });
            return projects.ToList();
        }

    }
}
