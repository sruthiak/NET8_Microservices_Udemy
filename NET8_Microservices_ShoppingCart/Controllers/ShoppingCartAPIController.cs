using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NET8_Microservices_ShoppingCartAPI.Data;
using NET8_Microservices_ShoppingCartAPI.Models;
using NET8_Microservices_ShoppingCartAPI.Models.DTOs;

namespace NET8_Microservices_ShoppingCartAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartAPIController : ControllerBase
    {
        private readonly AppDbContext appDbContext;
        private readonly IMapper mapper;
        private ResponseDTO responseDTO;

        public ShoppingCartAPIController(AppDbContext appDbContext,IMapper mapper)
        {
            this.appDbContext = appDbContext;
            this.mapper = mapper;
            this.responseDTO = new ResponseDTO();
        }
        /// <summary>
        /// Creating and updating a shopping cart entry
        /// Three scenarios
        /// 1. A user add an item for the first time, so first add Cart header, then cart details
        /// 2. User adds a new item to an already existing cart. So get the cart header id, then insert Cart details
        /// 3. User updates quantity of an existing item in cart, So get the details, then update the count on details
        /// </summary>
        /// <param name="cartDTO"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>

        [HttpPost("CartUpsert")]                

        public async Task<ResponseDTO> CartUpsert([FromBody] CartDTO cartDTO)
        {
            try
            {
                //get cart header by userId
                var cartHeaderFromDb = await appDbContext.CartHeaders.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == cartDTO.CartHeader.UserId);
                if (cartHeaderFromDb == null){
                    //create a new cart header and cartdetails.scenario 1
                    var cartHeader = mapper.Map<CartHeaderDTO, CartHeader>(cartDTO.CartHeader);
                    appDbContext.CartHeaders.Add(cartHeader);
                    await appDbContext.SaveChangesAsync(); //after saving it generates the headerId

                    //add headerId to cartDetailsDTO
                    cartDTO.CartDetails.First().CartHeaderId = cartHeader.Id;
                    var cartDetails = mapper.Map<CartDetailsDTO, CartDetails>(cartDTO.CartDetails.FirstOrDefault());
                    appDbContext.CartDetails.Add(cartDetails);
                    await appDbContext.SaveChangesAsync();
                }
                else
                {
                    //check id cartDetails exits for this headerId and productId
                    var cartDetailsFromDb = await appDbContext.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                        x => x.CartHeaderId == cartHeaderFromDb.Id 
                        && x.ProductId==cartDTO.CartDetails.FirstOrDefault().ProductId);
                    //AsNoTracking to avoid the error: The instance of entity type 'CartDetails' cannot
                    //be tracked because another instance with the same key value for {'Id'} is already
                    //being tracked. 

                    if (cartDetailsFromDb == null)
                    {
                        //insert new cartdetails. Scenario 2

                        //add headerId to cartDetailsDTO
                        cartDTO.CartDetails.First().CartHeaderId = cartHeaderFromDb.Id;

                        var cartDetails = mapper.Map<CartDetailsDTO, CartDetails>(cartDTO.CartDetails.FirstOrDefault());
                        appDbContext.CartDetails.Add(cartDetails);
                        await appDbContext.SaveChangesAsync();
                    }
                    else
                    {
                        //update the count.Scenario 3
                        cartDTO.CartDetails.First().Count += cartDetailsFromDb.Count;
                        cartDTO.CartDetails.First().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                        cartDTO.CartDetails.First().Id = cartDetailsFromDb.Id;

                        var cartDetails = mapper.Map<CartDetailsDTO, CartDetails>(cartDTO.CartDetails.FirstOrDefault());
                        appDbContext.CartDetails.Update(cartDetails);
                        await appDbContext.SaveChangesAsync();
                    }
                }
                responseDTO.Result = cartDTO;
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
