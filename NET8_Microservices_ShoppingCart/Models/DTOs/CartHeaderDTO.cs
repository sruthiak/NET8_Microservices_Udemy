﻿

namespace NET8_Microservices_ShoppingCartAPI.Models.DTOs
{
    public class CartHeaderDTO
    {
        
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? CouponCode { get; set; }

        
        public double Discount { get; set; }
        
        public double CartTotal { get; set; }
    }
}
