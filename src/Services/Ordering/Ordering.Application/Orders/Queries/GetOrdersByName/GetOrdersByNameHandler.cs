﻿using BuildingBlocks.CQRS;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Data;
using Ordering.Application.Extensions;

namespace Ordering.Application.Orders.Queries.GetOrdersByName
{
    public class GetOrdersByNameHandler(IApplicationDbContext dbContext) : IQueryHandler<GetOrdersByNameQuery, GetOrdersByNameResult>
    {
        public async Task<GetOrdersByNameResult> Handle(GetOrdersByNameQuery query, CancellationToken cancellationToken)
        {
            var orders = await dbContext.Orders.Include(s => s.OrderItems)
                .AsNoTracking()
                .Where(s => s.OrderName.Value.Contains(query.Name))
                .OrderBy(s => s.OrderName.Value)
                .ToListAsync(cancellationToken);
            var ordersDto = orders.ProjectToOrderDto();
            return new GetOrdersByNameResult(Orders: ordersDto);
        }


    }
}
