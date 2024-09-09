using BuildingBlocks.CQRS;
using Catalog.Api.Models;
using Marten;

namespace Catalog.Api.Products.GetProducts
{
    public record GetProductsQuery : IQuery<GetProductsResult>;

    public record GetProductsResult(IEnumerable<Product> Products);
    internal class GetProductsHandler(IQuerySession session, ILogger<GetProductsHandler> logger): IQueryHandler<GetProductsQuery,GetProductsResult>
    {
        public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetProductsHandler.Handle called with {@Query}", query);
            var products = await session.Query<Product>().ToListAsync(cancellationToken);
            return new GetProductsResult(products);
        }
    }
}
