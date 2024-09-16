using Basket.Api.Models;
using Carter;
using Mapster;
using MediatR;

namespace Basket.Api.Basket.StoreBasket
{
    public record StoreBasketRequest(ShoppingCard Card);

    public record StoreBasketResponse(string Username);
    public class StoreBasketEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/basket", async (StoreBasketRequest request, ISender sender) =>
            {
                var data = request.Adapt<StoreBasketCommand>();
                var result = await sender.Send(data);
                var response = result.Adapt<StoreBasketResponse>();
                return Results.Created($"/basket/{response.Username}", response);
            })
            .WithName("Create Basket")
            .Produces<StoreBasketResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Basket")
            .WithDescription("Create Basket");
        }
    }
}
