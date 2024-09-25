using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services
{
    public class DiscountService(DiscountDbContext dbContext, ILogger<DiscountService> logger) : DiscountProtoService.DiscountProtoServiceBase
    {
        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbContext.Coupons.FirstOrDefaultAsync(s => s.ProductName == request.ProductName) ?? new Coupon()
            {
                ProductName = "Not Found",
                Amount = 0,
                Description = "NotFound Product"
            };
            logger.LogInformation("Discount is retrieved for ProductName: {productName}", request.ProductName);
            var response = coupon.Adapt<CouponModel>();
            return response;
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();
            if (coupon is null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));
            await dbContext.Coupons.AddAsync(coupon, CancellationToken.None);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var couponModel = coupon.Adapt<CouponModel>();
            return couponModel;
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();
            if (coupon is null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));

            dbContext.Coupons.Update(coupon);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var couponModel = coupon.Adapt<CouponModel>();
            return couponModel;
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {

            var coupon = await dbContext.Coupons.FirstOrDefaultAsync(s => s.ProductName == request.ProductName);
            if (coupon is null)
                throw new RpcException(new Status(StatusCode.NotFound, "NotFound"));
            dbContext.Coupons.Remove(coupon);
            await dbContext.SaveChangesAsync();
            return new DeleteDiscountResponse
            {
                Success = true
            };
        }
    }
}
