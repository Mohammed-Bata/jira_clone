using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Columns.Commands.DeleteColumn
{
    public class DeleteColumnCommandHandler: IRequestHandler<DeleteColumnCommand>
    {
        private readonly IAppDbContext _context;

        public DeleteColumnCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteColumnCommand request, CancellationToken cancellationToken)
        {
            var column =  await _context.ProjectColumns.FindAsync(new object[] { request.ColumnId }, cancellationToken);
            if(column == null)
            {
                throw new Exception("Column not found");
            }
            _context.ProjectColumns.Remove(column);
            await _context.SaveChangesAsync(cancellationToken);
            return;
        }
    }
}
