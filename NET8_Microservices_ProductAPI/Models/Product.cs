using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;

namespace NET8_Microservices_ProductAPI.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string  Description { get; set; }
        [Required]
        [Range(10,1000)]
        public double Price { get; set; }
        public string ImageUrl { get; set; }
        public string CategoryName { get; set; }
    }
}
