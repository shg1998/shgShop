using BuildingBlocks.CQRS;
using Ordering.Application.Data;
using Ordering.Application.Dtos;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Application.Orders.Commands.CreateOrder
{
    public class CreateOrderHandler(IApplicationDbContext dbContext) : ICommandHandler<CreateOrderCommand, CreateOrderResult>
    {
        public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            var order = this.CreateNewOrder(command.Order);
            await dbContext.Orders.AddAsync(order, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            return new CreateOrderResult(order.Id.Value);
        }

        private Order CreateNewOrder(OrderDto commandOrder)
        {
            var shippingAddress = Address.Of(commandOrder.ShippingAddress.FirstName, commandOrder.ShippingAddress.LastName,
                commandOrder.ShippingAddress.EmailAddress, commandOrder.ShippingAddress.AddressLine,
                commandOrder.ShippingAddress.Country, commandOrder.ShippingAddress.State,
                commandOrder.ShippingAddress.ZipCode);

            var billingAddress = Address.Of(commandOrder.BillingAddress.FirstName, commandOrder.BillingAddress.LastName,
                commandOrder.BillingAddress.EmailAddress, commandOrder.BillingAddress.AddressLine,
                commandOrder.BillingAddress.Country, commandOrder.BillingAddress.State,
                commandOrder.BillingAddress.ZipCode);

            var newOrder = Order.Create(
                id: OrderId.Of(Guid.NewGuid()),
                customerId: CustomerId.Of(commandOrder.CustomerId),
                orderName: OrderName.Of(commandOrder.OrderName),
                shippingAddress: shippingAddress,
                billingAddress: billingAddress,
                payment: Payment.Of(commandOrder.Payment.CardName, commandOrder.Payment.CardNumber,
                    commandOrder.Payment.Expiration, commandOrder.Payment.Cvv, commandOrder.Payment.PaymentMethod)
            );

            foreach (var orderItem in commandOrder.OrderItems)
                newOrder.Add(ProductId.Of(orderItem.ProductId), orderItem.Quantity, orderItem.Price);

            return newOrder;
        }
    }
}
