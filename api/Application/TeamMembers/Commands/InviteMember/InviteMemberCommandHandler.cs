using Application.Interfaces;
using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.TeamMembers.Commands.InviteMember
{
    public class InviteMemberCommandHandler:IRequestHandler<InviteMemberCommand,bool>
    {
        private readonly IAppDbContext _context;
        private IEmailService _emailService;

        public InviteMemberCommandHandler(IAppDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<bool> Handle(InviteMemberCommand request, CancellationToken cancellationToken)
        {
            var Invitation = new Invitation
            {
                ProjectId = request.ProjectId,
                Email = request.Email,
                Token = Guid.NewGuid(),
            };

            _context.Invitations.AddAsync(Invitation);
            await _context.SaveChangesAsync(cancellationToken);

            var invitationLink = $"http://localhost:4200/accept?token={Invitation.Token}";

            await _emailService.SendEmail(Invitation.Email,invitationLink);

            return true;
        }
    }
}
