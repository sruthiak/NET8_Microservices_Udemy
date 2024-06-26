using Microservices.Web.Models;

namespace Microservices.Web.Interfaces
{
    public interface ICouponService
    {
        Task<ResponseDTO> GetCouponsAsync();
        Task<ResponseDTO> GetCouponAsync(int id);
        Task<ResponseDTO> CreateCouponAsync(CouponDTO couponDTO);
        Task<ResponseDTO> UpdateCouponAsync(CouponDTO couponDTO);
        Task<ResponseDTO> DeleteCouponAsync(int id);

    }
}
