

namespace NET8_Microservices_ShoppingCartAPI.Models.DTOs
{
    public class CartDetailsDTO
    {
        public int Id { get; set; }
        public int CartHeaderId { get; set; }
        public CartHeaderDTO? CartHeader { get; set; }
        public int ProductId { get; set; }
      
        public ProductDTO? Product { get; set; } // to get the product details by calling ProductAPI
        public int Count { get; set; }
    }
}
