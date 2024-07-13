using Microservices.Web.Interfaces;
using Microservices.Web.Models;
using Microservices.Web.Utilities;

namespace Microservices.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly IBaseService baseService;
        private RequestDTO requestDTO = new();
        private ResponseDTO responseDTO;

        public ProductService(IBaseService baseService)
        {
            this.baseService = baseService;
        }

        public async Task<ResponseDTO> CreateProductAsync(ProductDTO productDTO)
        {
            try
            {
                requestDTO.HttpClientName = "ProductClient";
                requestDTO.RequestUri = $"api/ProductAPI/CreateProduct";
                requestDTO.APIType = Common.APIType.POST;
                requestDTO.Data = productDTO;
                responseDTO = await baseService.SendAsync(requestDTO);
            }
            catch (Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
            }
            return responseDTO;

        }


        public async Task<ResponseDTO> DeleteProductAsync(int id)
        {
            try
            {
                requestDTO.HttpClientName = "ProductClient";
                requestDTO.RequestUri = $"api/ProductAPI/DeleteProduct/{id}";
                requestDTO.APIType = Common.APIType.DELETE;
                responseDTO = await baseService.SendAsync(requestDTO);
            }
            catch (Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
            }
            return responseDTO;
        }


        public async Task<ResponseDTO> GetProductByIdAsync(int id)
        {

            try
            {
                requestDTO.HttpClientName = "ProductClient";
                requestDTO.RequestUri = $"api/ProductAPI/GetProductById/{id}";
                requestDTO.APIType = Common.APIType.GET;
                responseDTO = await baseService.SendAsync(requestDTO);
            }
            catch (Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
            }
            return responseDTO;

        }

        public async Task<ResponseDTO> GetProductsAsync()
        {
            try
            {
                requestDTO.HttpClientName = "ProductClient";
                requestDTO.RequestUri = @"api/ProductAPI";
                requestDTO.APIType = Common.APIType.GET;
                responseDTO = await baseService.SendAsync(requestDTO);
            }
            catch (Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> UpdateProductAsync(ProductDTO productDTO)
        {
            try
            {
                requestDTO.HttpClientName = "ProductClient";
                requestDTO.RequestUri = $"api/ProductAPI/UpdateProduct";
                requestDTO.APIType = Common.APIType.PUT;
                requestDTO.Data = productDTO;
                responseDTO = await baseService.SendAsync(requestDTO);
            }
            catch (Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
            }
            return responseDTO;
        }

        
    }
}
