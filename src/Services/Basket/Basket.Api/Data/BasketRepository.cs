using Basket.Api.Exceptions;
using Basket.Api.Models;
using Marten;

namespace Basket.Api.Data
{
    public class BasketRepository(IDocumentSession session): IBasketRepository
    {
        public async Task<ShoppingCard> GetBasket(string username, CancellationToken cancellationToken = default)
        {
            var basket = await session.LoadAsync<ShoppingCard>(username, cancellationToken);
            return basket ?? throw new BasketNotFoundException(username);
        }

        public async Task<ShoppingCard> StoreBasket(ShoppingCard basket, CancellationToken cancellationToken = default)
        {
            session.Store(basket);
            await session.SaveChangesAsync(cancellationToken);
            return basket;
        }

        public async Task<bool> DeleteBasket(string username, CancellationToken cancelToken = default)
        {
            session.Delete<ShoppingCard>(username);
            await session.SaveChangesAsync(cancelToken);
            return true;
        }

    }
}
