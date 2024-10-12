using BuildingBlocks.CQRS;
using Ordering.Application.Data;
using Ordering.Application.Dtos;
using Ordering.Application.Exceptions;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Application.Orders.Commands.UpdateOrder
{
    public class UpdateOrderHandler(IApplicationDbContext dbContext) : ICommandHandler<UpdateOrderCommand, UpdateOrderResult>
    {
        public async Task<UpdateOrderResult> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
        {
            var orderId = OrderId.Of(command.Order.Id);
            var order = await dbContext.Orders.FindAsync([orderId], cancellationToken);
            if (order is null)
                throw new OrderNotFoundException(command.Order.Id);

            this.UpdateOrderWithNewValues(order, command.Order);
            dbContext.Orders.Update(order);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new UpdateOrderResult(true);
        }

        private void UpdateOrderWithNewValues(Order order, OrderDto commandOrder)
        {
            var updatedShippingAddress = Address.Of(commandOrder.ShippingAddress.FirstName,
                commandOrder.ShippingAddress.LastName, commandOrder.ShippingAddress.EmailAddress,
                commandOrder.ShippingAddress.AddressLine, commandOrder.ShippingAddress.Country,
                commandOrder.ShippingAddress.State, commandOrder.ShippingAddress.ZipCode);

            var updatedBillingAddress = Address.Of(commandOrder.BillingAddress.FirstName,
                commandOrder.BillingAddress.LastName, commandOrder.BillingAddress.EmailAddress,
                commandOrder.BillingAddress.AddressLine, commandOrder.BillingAddress.Country,
                commandOrder.BillingAddress.State, commandOrder.BillingAddress.ZipCode);

            var updatedPayment = Payment.Of(commandOrder.Payment.CardName, commandOrder.Payment.CardNumber,
                commandOrder.Payment.Expiration, commandOrder.Payment.Cvv, commandOrder.Payment.PaymentMethod);

            order.Update(
                orderName: OrderName.Of(commandOrder.OrderName),
                shippingAddress: updatedShippingAddress, 
                billingAddress: updatedBillingAddress,
                payment: updatedPayment, 
                status: commandOrder.Status
                );
        }
    }
}
