using BuildingBlocks.CQRS;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Data;
using Ordering.Application.Extensions;
using Ordering.Domain.ValueObjects;

namespace Ordering.Application.Orders.Queries.GetOrderByCustomer
{
    public class GetOrderByCustomerHandler(IApplicationDbContext dbContext) : IQueryHandler<GetOrderByCustomerQuery, GetOrderByCustomerResult>
    {
        public async Task<GetOrderByCustomerResult> Handle(GetOrderByCustomerQuery query, CancellationToken cancellationToken)
        {
            var orders = await dbContext.Orders.Include(s=>s.OrderItems)
                .AsNoTracking()
                .Where(s=>s.CustomerId == CustomerId.Of(query.CustomerId))
                .OrderBy(s=>s.OrderName.Value)
                .ToListAsync(cancellationToken);

            var data = orders.ProjectToOrderDto();
            return new GetOrderByCustomerResult(data);
        }
    }
}
