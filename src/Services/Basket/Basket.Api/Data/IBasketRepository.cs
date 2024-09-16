using Basket.Api.Models;

namespace Basket.Api.Data
{
    public interface IBasketRepository
    {
        Task<ShoppingCard> GetBasket(string username, CancellationToken cancellationToken = default);
        Task<ShoppingCard> StoreBasket(ShoppingCard basket, CancellationToken cancellationToken = default);
        Task<bool> DeleteBasket(string username, CancellationToken cancelToken = default);
    }
}
