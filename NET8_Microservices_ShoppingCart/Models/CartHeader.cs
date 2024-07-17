﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NET8_Microservices_ShoppingCartAPI.Models
{
    public class CartHeader
    {
        [Key]
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? CouponCode { get; set; }

        [NotMapped]
        public double  Discount { get; set; }
        [NotMapped]
        public double CartTotal { get; set; }
    }
}
