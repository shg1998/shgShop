using BuildingBlocks.CQRS;
using Ordering.Application.Dtos;

namespace Ordering.Application.Orders.Queries.GetOrderByCustomer
{
    public record GetOrderByCustomerQuery(Guid CustomerId) : IQuery<GetOrderByCustomerResult>;
    public record GetOrderByCustomerResult(IEnumerable<OrderDto> Order);
}
