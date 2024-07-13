using Microservices.Web.Models;

namespace Microservices.Web.Interfaces
{
    public interface IProductService
    {
        Task<ResponseDTO> GetProductsAsync();
        Task<ResponseDTO> GetProductByIdAsync(int id);
        Task<ResponseDTO> CreateProductAsync(ProductDTO productDTO);
        Task<ResponseDTO> UpdateProductAsync(ProductDTO productDTO);
        Task<ResponseDTO> DeleteProductAsync(int id);


    }
}
