using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.TeamMembers.Commands.InviteMember
{
    public record InviteMemberCommand(
        string Email,
        int ProjectId
    ):IRequest<bool>;
   
}
