using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using NET8_Microservices_ProductAPI.Data;
using NET8_Microservices_ProductAPI.Models;
using NET8_Microservices_ProductAPI.Models.DTOs;

namespace NET8_Microservices_ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize] - commenting bcz Get method is accessed in HomeController
    public class ProductAPIController : ControllerBase
    {
        private readonly AppDbContext appDbContext;
        private readonly IMapper mapper;
        private ResponseDTO responseDTO=new();

        public ProductAPIController(AppDbContext appDbContext,IMapper mapper)
        {
            this.appDbContext = appDbContext;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<ResponseDTO> GetProducts()
        {
            
            try
            {
                IEnumerable<Product> productList = await appDbContext.Products.ToListAsync();
                IEnumerable<ProductDTO> productDTOList=mapper.Map<IEnumerable<ProductDTO>>(productList);

                responseDTO.Result=productDTOList;

            }
            catch(Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
            }
            return responseDTO;
        }

        [HttpGet]
        [Route("GetProductById/{id:int}")]
        public async Task<ResponseDTO> GetProductById(int id)
        {

            try
            {
                Product product = await appDbContext.Products.FirstAsync(x=>x.Id==id);
                
                    ProductDTO productDTO = mapper.Map<ProductDTO>(product);

                    responseDTO.Result = productDTO;
                

            }
            catch (Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
            }
            return responseDTO;
        }

        [HttpPost("CreateProduct")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ResponseDTO> CreateProduct([FromBody] ProductDTO productDto)
        {
            try
            {
                if (productDto != null)
                {
                    Product product = mapper.Map<Product>(productDto);
                    await appDbContext.Products.AddAsync(product);
                    await appDbContext.SaveChangesAsync();
                    responseDTO.Result = product;
                }
                else
                {
                    responseDTO.IsSuccess = false;
                    responseDTO.Message = "Empty Product";
                }
            }
            catch(Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
            }
            return responseDTO;
        }

        [HttpPut("UpdateProduct")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ResponseDTO> UpdateProduct([FromBody] ProductDTO productDto)
        {
            try
            {
                if (productDto != null)
                {
                    Product product = mapper.Map<Product>(productDto);
                    appDbContext.Products.Update(product);
                    await appDbContext.SaveChangesAsync();
                    responseDTO.Result = product;
                }
                else
                {
                    responseDTO.IsSuccess = false;
                    responseDTO.Message = "Product not found";
                }
            }
            catch (Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
            }
            return responseDTO;
        }

        [HttpDelete]
        [Route("DeleteProduct/{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ResponseDTO> DeleteProduct(int id)
        {

            try
            {
                Product product = await appDbContext.Products.FirstAsync(x => x.Id == id);
                
                    appDbContext.Products.Remove(product);
                    await appDbContext.SaveChangesAsync();
                

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
