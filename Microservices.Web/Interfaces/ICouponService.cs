using Microservices.Web.Models;

namespace Microservices.Web.Interfaces
{
    public interface ICouponService
    {
        Task<ResponseDTO> GetCoupons();
        Task<ResponseDTO> GetCoupon(int id);
        Task<ResponseDTO> CreateCoupon(CouponDTO couponDTO);
        Task<ResponseDTO> UpdateCoupon(CouponDTO couponDTO);
        Task<ResponseDTO> DeleteCoupon(int id);

    }
}
