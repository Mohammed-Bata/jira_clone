using Application.DTOs;
using Application.Notifications.Commands.MarkAllRead;
using Application.Notifications.Queries.GetNotificationsByStatus;
using Application.Notifications.Queries.GetUnreadCount;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public NotificationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("unread")]
        public async Task<ActionResult<int>> GetUnreadCount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var query = new GetUnreadCountQuery(userId);
            var result = await _mediator.Send(query);
            return Ok(new
            {
                unread = result
            });
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<NotificationDto>>> GetAllByStatus([FromQuery] string Status, [FromQuery] int Page = 1, [FromQuery] int PageSize = 5)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var query = new GetNotificationsByStatusQuery(userId,Status, Page, PageSize);

            var result = await _mediator.Send(query);

            return result;
        }

        [HttpPatch("markallread")]
        public async Task<ActionResult> MarkAllRead()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var command = new MarkAllNotificationsReadCommand(userId);
            var result = await _mediator.Send(command);

            if (!result)
            {
                return BadRequest();
            }

            return NoContent();
        }

    }
}
