using Basket.Api.Data;
using Basket.Api.Models;
using BuildingBlocks.CQRS;

namespace Basket.Api.Basket.GetBasket
{
    public record GetBasketQuery(string username) : IQuery<GetBasketResult>;

    public record GetBasketResult(ShoppingCard Card);

    public class GetBasketHandler(IBasketRepository repository): IQueryHandler<GetBasketQuery,GetBasketResult>
    {
        public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
        {
            var basket = await repository.GetBasket(query.username, cancellationToken);
            return new GetBasketResult(basket);
        }
    }
}
