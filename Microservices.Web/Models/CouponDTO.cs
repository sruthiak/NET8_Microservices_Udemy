using System.ComponentModel.DataAnnotations;

namespace Microservices.Web.Models
{
    public class CouponDTO
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public double DiscountAmount { get; set; }
        public int MinAmount { get; set; }
    }
}
