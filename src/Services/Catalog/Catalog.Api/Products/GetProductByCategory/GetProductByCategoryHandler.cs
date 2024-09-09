using BuildingBlocks.CQRS;
using Catalog.Api.Models;
using Marten;

namespace Catalog.Api.Products.GetProductByCategory
{
    public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;

    public record GetProductByCategoryResult(IEnumerable<Product> Products);
    internal class GetProductByCategoryHandler(IQuerySession session, ILogger<GetProductByCategoryHandler> logger): IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
    {
        public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetProductByCategoryHandler.Handle called with {@Query}", query);
            var products = await session.Query<Product>().Where(s => s.Categories.Contains(query.Category))
                .ToListAsync(cancellationToken);
            return new GetProductByCategoryResult(products);
        }
    }
}
