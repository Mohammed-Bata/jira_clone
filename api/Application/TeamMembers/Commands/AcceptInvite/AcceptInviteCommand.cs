using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.TeamMembers.Commands.AcceptInvite
{
    public record AcceptInviteCommand(Guid Token,string UserId):IRequest<bool>;
    
}
