using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.TeamMembers.Commands.AcceptInvite
{
    public class AcceptInviteCommandHandler:IRequestHandler<AcceptInviteCommand,bool>
    {
        private readonly IAppDbContext _context;

        public AcceptInviteCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(AcceptInviteCommand command, CancellationToken cancellationToken)
        {
            var invite = await _context.Invitations.FirstOrDefaultAsync(i => i.Token == command.Token, cancellationToken);

            if (invite == null || invite.ExpiresAt < DateTimeOffset.UtcNow)
            {
                return false;
            }

            var member = new ProjectMember
            {
                ProjectId = invite.ProjectId,
                UserId = command.UserId,
                Role = ProjectRole.Member
            };
            _context.ProjectMembers.Add(member);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
