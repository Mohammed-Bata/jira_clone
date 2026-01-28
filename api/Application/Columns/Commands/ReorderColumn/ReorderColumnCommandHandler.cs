using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Columns.Commands.ReorderColumn
{
    public class ReorderColumnCommandHandler: IRequestHandler<ReorderColumnCommand,double>
    {
        private readonly IAppDbContext _context;
        private const double Threshold = 0.0001; // Minimum gap before rebalance
        public ReorderColumnCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<double> Handle(ReorderColumnCommand request, CancellationToken cancellationToken)
        {
            var column = await _context.ProjectColumns.FindAsync(new object[] { request.ColumnId }, cancellationToken);
            if (column == null)
            {
                throw new Exception("Column not found");
            }
            double newOrder;
            if (request.PrevOrder == null)
            {
                newOrder = (request.NextOrder ?? 100) / 2.0;
            }else if(request.NextOrder == null)
            {
                newOrder = (request.PrevOrder ?? 0) + 100;
            }
            else
            {
                newOrder = (request.PrevOrder.Value + request.NextOrder.Value) / 2.0;
            }
            column.Order = newOrder;
            await _context.SaveChangesAsync(cancellationToken);
            // Check if we need to rebalance (The "Safety Valve")
            if (request.PrevOrder.HasValue && Math.Abs(newOrder - request.PrevOrder.Value) < Threshold)
            {
                await RebalanceOrders(column.ProjectId);
            }
            return column.Order;
        }

        private async Task RebalanceOrders(int projectId)
        {
            var items = _context.ProjectColumns
                .Where(wi => wi.ProjectId == projectId)
                .OrderBy(wi => wi.Order)
                .ToList();

            for (int i = 0; i < items.Count; i++)
            {
                items[i].Order = (i + 1) * 100;
            }

            await _context.SaveChangesAsync();
        }
    }
}
