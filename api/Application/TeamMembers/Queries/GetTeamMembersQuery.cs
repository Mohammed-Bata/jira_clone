using Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.TeamMembers.Queries
{
    public record GetTeamMembersQuery(int ProjectId):IRequest<List<UserDto>>;

}
