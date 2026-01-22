using Application.Interfaces;
using Dapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Projects.Queries.GetProject
{
    public class GetProjectQueryHandler: IRequestHandler<GetProjectQuery, ProjectDto>
    {
        private ISqlConnectionFactory _sqlConnectionFactory;

        public GetProjectQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<ProjectDto> Handle(GetProjectQuery request, CancellationToken cancellationToken)
        { 
            using var connection = _sqlConnectionFactory.CreateConnection();

            // Implementation to retrieve project data from the database
            var sql = @"SELECT p.Id,p.Name,pc.Id,pc.Title,pc.[Order],i.Id,i.Title,i.[Order],i.Priority,i.DueDate FROM Projects p 
                JOIN ProjectColumns pc ON p.Id = pc.ProjectId LEFT JOIN WorkItems i ON pc.Id = i.ProjectColumnId
                WHERE p.Id = @ProjectId
                ORDER BY pc.[Order] ASC,i.[Order] ASC;";


            var ProjectDictionary = new Dictionary<int, ProjectDto>();
            var ColumnsDictionary = new Dictionary<int, ProjectColumnDto>();


            var result = await connection.QueryAsync<ProjectDto, ProjectColumnDto, WorkItemDto, ProjectDto>(sql,

              (p, pc, i) =>
              {
                  if (!ProjectDictionary.TryGetValue(p.Id,out var project))
                  {
                      project = p;
                      project.Columns = new List<ProjectColumnDto>();
                      ProjectDictionary.Add(p.Id, project);
                  }

                  if(!ColumnsDictionary.TryGetValue(pc.Id,out var column))
                  {
                      column = pc;
                      column.WorkItems = new List<WorkItemDto>();
                      ColumnsDictionary.Add(pc.Id, column);

                      project.Columns.Add(column);
                  }

                  column.WorkItems.Add(i);

                  return project;
              },
              new {ProjectId = request.ProjectId},
              splitOn:"Id,Id"          
                );


            return result.FirstOrDefault();
        }
    }
}
