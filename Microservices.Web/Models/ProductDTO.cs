using System.ComponentModel.DataAnnotations;

namespace Microservices.Web.Models
{
    public class ProductDTO
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Range(10, 1000)]
        public double Price { get; set; }
        public string ImageUrl { get; set; }
        public string CategoryName { get; set; }

        [Range(1,100)]
        public int Count { get; set; } // this field is in the DTO, not in actual model
    }
}
