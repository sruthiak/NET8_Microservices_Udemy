using NET8_Microservices_ShoppingCartAPI.Models.DTOs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NET8_Microservices_ShoppingCartAPI.Models
{
    public class CartDetails
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(CartHeaderId))]
        public int CartHeaderId { get; set; }
        public CartHeader CartHeader { get; set; }
        public int ProductId { get; set; }
        [NotMapped]
        public ProductDTO  Product { get; set; } // to get the product details by calling ProductAPI
        public int Count { get; set; }
    }
}
