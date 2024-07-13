using System.ComponentModel.DataAnnotations;

namespace NET8_Microservices_ProductAPI.Models.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
       
        public string Description { get; set; }
        
        public double Price { get; set; }
        public string ImageUrl { get; set; }
        public string CategoryName { get; set; }
    }
}
