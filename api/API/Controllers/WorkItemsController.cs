using Application.DTOs;
using Application.WorkItems.Commands.CreateWorkItem;
using Application.WorkItems.Commands.DeleteWorkItem;
using Application.WorkItems.Commands.ReorderWorkItem;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkItemsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WorkItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteWorkItem(int id)
        {
            var command = new DeleteWorkItemCommand(id);
            await _mediator.Send(command);

            return Ok();
        }

        [HttpPatch("reorder")]
        public async Task<ActionResult> ReorderWorkItems(ReorderWorkItemsDto dto)
        {
            var command = new ReorderWorkItemCommand(dto.WorkItemId, dto.PrevOrder, dto.NextOrder);

            var newOrder = await _mediator.Send(command);

            return Ok(new {order = newOrder});
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetWorkItem(int id)
        {
            // Implementation for retrieving a work item by ID
            return Ok();
        }

        [HttpPost("create")]
        public async Task<ActionResult<CreateWorkItemResult>> CreateWorkItem(CreateWorkItemDto dto)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var command = new CreateWorkItemCommand(dto.Title,dto.Description,dto.ProjectColumnId,dto.AssignedToUserId,userId,dto.Priority,dto.DueDate,dto.Type);
            var result = await _mediator.Send(command);
            return result;

        }
    }
}
