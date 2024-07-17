

namespace NET8_Microservices_Udemy.Services.ShoppingCartAPI.Models.DTOs
{
    public class CouponDTO
    {
        
        public int Id { get; set; }
        public string Code { get; set; }
        public double DiscountAmount { get; set; }
        public int MinAmount { get; set; }
    }
}
