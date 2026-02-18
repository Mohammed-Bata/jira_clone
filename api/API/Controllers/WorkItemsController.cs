using Application.DTOs;
using Application.WorkItems.Commands.CreateWorkItem;
using Application.WorkItems.Commands.DeleteWorkItem;
using Application.WorkItems.Commands.ReorderWorkItem;
using Application.WorkItems.Commands.UpdateWorkItem;
using Application.WorkItems.Queries.GetWorkItem;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Authorize]
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
            var command = new ReorderWorkItemCommand(dto.WorkItemId, dto.ColumnId,dto.PrevOrder, dto.NextOrder);

            var newOrder = await _mediator.Send(command);

            return Ok(new {order = newOrder});
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WorkItemDto>> GetWorkItem(int id)
        {
            var query = new GetWorkItemQuery(id);
            var workItem = await _mediator.Send(query);
    
            if (workItem == null)
            {
                return NotFound();
            }
    
            return Ok(workItem);
        }

        [HttpPost("create")]
        public async Task<ActionResult<CreateWorkItemResult>> CreateWorkItem(CreateWorkItemDto dto)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.FindFirstValue(ClaimTypes.Name);

            var command = new CreateWorkItemCommand(userName,dto.Title,dto.Description,dto.ProjectColumnId,dto.AssignedToUserId,dto.AssignedToUserName,userId,dto.Priority,dto.DueDate,dto.Type);
            var result = await _mediator.Send(command);
            return result;

        }


        [HttpPatch("{id}")]
        public async Task<ActionResult<bool>> UpdateWorkItem(int id, UpdateWorkItemDto dto)
        {
            var command = new UpdateWorkItemCommand(id,dto);

            var result = await _mediator.Send(command);
            return result;
        }
    }
}
