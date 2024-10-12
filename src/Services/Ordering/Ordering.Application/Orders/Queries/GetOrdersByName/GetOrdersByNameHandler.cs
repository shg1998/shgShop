using BuildingBlocks.CQRS;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Data;
using Ordering.Application.Dtos;
using Ordering.Domain.Models;

namespace Ordering.Application.Orders.Queries.GetOrdersByName
{
    public class GetOrdersByNameHandler(IApplicationDbContext dbContext) : IQueryHandler<GetOrdersByNameQuery, GetOrdersByNameResult>
    {
        public async Task<GetOrdersByNameResult> Handle(GetOrdersByNameQuery query, CancellationToken cancellationToken)
        {
            var orders = await dbContext.Orders.Include(s => s.OrderItems)
                .AsNoTracking()
                .Where(s => s.OrderName.Value.Contains(query.Name))
                .OrderBy(s => s.OrderName)
                .ToListAsync(cancellationToken);
            var ordersDto = this.ProjectToOrderDto(orders);
            return new GetOrdersByNameResult(Orders: ordersDto);
        }

        private IEnumerable<OrderDto> ProjectToOrderDto(IEnumerable<Order> orders)
        {
            List<OrderDto> dtos = [];
            dtos.AddRange(orders.Select(order => new OrderDto(Id: order.Id.Value, CustomerId: order.CustomerId.Value,
                OrderName: order.OrderName.Value,
                ShippingAddress: new AddressDto(FirstName: order.ShippingAddress.FirstName,
                    LastName: order.ShippingAddress.LastName, EmailAddress: order.ShippingAddress.EmailAddress!,
                    AddressLine: order.ShippingAddress.AddressLine, Country: order.ShippingAddress.Country,
                    State: order.ShippingAddress.State, ZipCode: order.ShippingAddress.ZipCode),
                BillingAddress: new AddressDto(FirstName: order.BillingAddress.FirstName,
                    LastName: order.BillingAddress.LastName, EmailAddress: order.BillingAddress.EmailAddress!,
                    AddressLine: order.BillingAddress.AddressLine, Country: order.BillingAddress.Country,
                    State: order.BillingAddress.State, ZipCode: order.BillingAddress.ZipCode),
                Payment: new PaymentDto(CardName: order.Payment.CardName!, CardNumber: order.Payment.CardNumber,
                    Expiration: order.Payment.Expiration, Cvv: order.Payment.CVV,
                    PaymentMethod: order.Payment.PaymentMethod), Status: order.Status,
                OrderItems: order.OrderItems.Select(oi =>
                    new OrderItemDto(oi.OrderId.Value, oi.ProductId.Value, oi.Quantity, oi.Price)).ToList())));

            return dtos;
        }
    }
}
