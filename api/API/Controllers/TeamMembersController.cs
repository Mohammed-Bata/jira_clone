using Application.DTOs;
using Application.TeamMembers.Commands.AcceptInvite;
using Application.TeamMembers.Commands.InviteMember;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamMembersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TeamMembersController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("invite")]
        public async Task<ActionResult> InviteMemberToProject([FromBody] InvitationDto dto)
        {
            var command = new InviteMemberCommand(dto.Email,dto.ProjectId);

            var result = await _mediator.Send(command);

            return Ok(new { message = "Invitation sent successfully!" });
        }

        [HttpPost("accept")]
        public async Task<ActionResult> AcceptInvite([FromBody] AcceptInvitationDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var command = new AcceptInviteCommand(dto.token, userId);

            var result = await _mediator.Send(command);

            return Ok(new { message = "Welcome to the team!" });

        }
    }
}
