using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.WorkItems.Commands.ReorderWorkItem
{
    public class ReorderWorkItemCommandHandler:IRequestHandler<ReorderWorkItemCommand,double>
    {
        private readonly IAppDbContext _context;
        private const double Threshold = 0.0001; // Minimum gap before rebalance
        public ReorderWorkItemCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<double> Handle(ReorderWorkItemCommand request, CancellationToken cancellationToken)
        {
            var workItem = await _context.WorkItems.FindAsync(new object[] { request.WorkItemId }, cancellationToken);
            if (workItem == null)
            {
                throw new Exception("Work item not found");
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

            workItem.Order = newOrder;
            await _context.SaveChangesAsync(cancellationToken);

            // Check if we need to rebalance (The "Safety Valve")
            if (request.PrevOrder.HasValue && Math.Abs(newOrder - request.PrevOrder.Value) < Threshold)
            {
                await RebalanceOrders(workItem.ProjectColumnId);
            }

            return workItem.Order;
        }

        private async Task RebalanceOrders(int columnId)
        {
            var items = _context.WorkItems
                .Where(wi => wi.ProjectColumnId == columnId)
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
