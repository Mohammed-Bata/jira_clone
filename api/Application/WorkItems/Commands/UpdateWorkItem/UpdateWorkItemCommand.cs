using Application.DTOs;
using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.WorkItems.Commands.UpdateWorkItem
{
    public record UpdateWorkItemCommand(int Id,UpdateWorkItemDto dto):IRequest<bool>;
    
}
