using Application.DTOs;
using Application.Projects.Commands.CreateProject;
using Application.Projects.Commands.DeleteProject;
using Application.Projects.Queries.GetProject;
using Application.Projects.Queries.GetProjects;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ProjectDto = Application.Projects.Queries.GetProject.ProjectDto;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<ProjectsDto>>> GetProjects()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var query = new GetProjectsQuery(userId!);
            var projects = await _mediator.Send(query);

            return projects;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDto>> GetProject(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var query = new GetProjectQuery(id, userId!);
            var project = await _mediator.Send(query);

            return project;
        }


        [HttpPost("create")]
        public async Task<ActionResult<ProjectsDto>> CreateProject(CreateProjectDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var command = new CreateProjectCommand(dto.Name, dto.Description, userId!);
            var project = await _mediator.Send(command);
            return project;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProject(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var command = new DeleteProjectCommand(id, userId!);

            var result = await _mediator.Send(command);

            if(!result)
            {
                return BadRequest("Failed to delete the project. It may not exist or you may not have permission to delete it.");
            }

            return NoContent();
        }

    }
}
