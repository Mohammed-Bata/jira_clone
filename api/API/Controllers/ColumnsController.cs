using Application.Columns.Commands.CreateColumn;
using Application.Columns.Commands.DeleteColumn;
using Application.Columns.Commands.ReorderColumn;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ColumnsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ColumnsController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpDelete]
        public async Task<ActionResult> DeleteColumn(DeleteColumnDto dto)
        {
            var command = new DeleteColumnCommand(dto.ColumnId,dto.TargetColumnId,dto.WorkItemIds);

            await _mediator.Send(command);

            return NoContent();
        }

        [HttpPost("create")]
        public async Task<ActionResult<CreateColumnResult>> CreateColumn(CreateColumnDto dto)
        {
            var command = new CreateColumnCommand(dto.Title,dto.ProjectId);

            var result = await _mediator.Send(command);

            return result;
        }

        [HttpPatch("reorder")]
        public async Task<ActionResult> ReorderColumns(ReorderColumnDto dto)
        {
            var command = new ReorderColumnCommand(dto.ColumnId, dto.PrevOrder, dto.NextOrder);
            var result = await _mediator.Send(command);
            return Ok(new {Order=result});
        }
    }
}
