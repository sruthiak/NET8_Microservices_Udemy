namespace NET8_Microservices_Udemy.Services.CouponAPI.Models.DTOs
{
    public class CouponDTO
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public double DiscountAmount { get; set; }
        public int MinAmount { get; set; }
    }
}
