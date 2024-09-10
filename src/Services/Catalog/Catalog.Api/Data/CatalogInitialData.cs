using Catalog.Api.Models;
using Marten;
using Marten.Schema;

namespace Catalog.Api.Data
{
    public class CatalogInitialData: IInitialData
    {
        public async Task Populate(IDocumentStore store, CancellationToken cancellation)
        {
            await using var session = store.LightweightSession();
            if(await session.Query<Product>().AnyAsync(cancellation)) return;
            session.Store(GetPreconfiguredProducts());
            await session.SaveChangesAsync(cancellation);
        }

        private static IEnumerable<Product> GetPreconfiguredProducts()
        {
            return new List<Product>
            {
                new()
                {
                    Id = new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff61"),
                    Name = "Iphone",
                    Description = "this is iphone!",
                    ImageFile = "product-1.png",
                    Price = 950.00M,
                    Categories = ["SmartPhone"]
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Samsung s10",
                    Description = "this is S10!",
                    ImageFile = "product-2.png",
                    Price = 750.00M,
                    Categories = ["SmartPhone"]
                }
            };
        }
    }
}
