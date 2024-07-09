using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NET8_Microservices_Udemy.Services.CouponAPI.Data;
using NET8_Microservices_Udemy.Services.CouponAPI.Models;
using NET8_Microservices_Udemy.Services.CouponAPI.Models.DTOs;
using System;

namespace NET8_Microservices_Udemy.Services.CouponAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
    //[Authorize]
	public class CouponAPIController : ControllerBase
	{
		private readonly AppDbContext appDbContext;
        private readonly IMapper mapper;
        private readonly ResponseDTO responseDTO;

		public CouponAPIController(AppDbContext appDbContext,IMapper mapper)
		{
			this.appDbContext = appDbContext;
            this.mapper = mapper;
            //ResponseDTO is used to return a standard result format from API
            responseDTO = new ResponseDTO();
		}		

		[HttpGet]
		public ResponseDTO Get()
		{
			try
			{
				IEnumerable<Coupon> coupons = appDbContext.Coupons.ToList();
                IEnumerable<CouponDTO> couponDTOs=mapper.Map<IEnumerable<CouponDTO>>(coupons);
				responseDTO.Result = couponDTOs;
				
			}
			catch(Exception ex)
			{
				responseDTO.Message = ex.Message;
				responseDTO.IsSuccess = false;
			}
			return responseDTO;
		}
		[HttpGet]
		[Route("{id:int}")]
		public ResponseDTO GetCoupon(int id)
		{
			try
			{
				Coupon coupon = appDbContext.Coupons.First(x => x.Id == id); //FirstorDefault can return null also.
																			 //So use First. If nothing then throws error
				responseDTO.Result = mapper.Map<CouponDTO>(coupon); //This is type of CoupunDTO
			}
			catch(Exception ex)
			{
				responseDTO.IsSuccess = false;
				responseDTO.Message = ex.Message;
			}
			return responseDTO;
		}

        [HttpGet]
        [Route("GetByCode/{code}")]
        public ResponseDTO GetCoupon(string code)
        {
            try
            {
                Coupon coupon = appDbContext.Coupons.First(x => x.Code.ToLower()==code.ToLower());
                responseDTO.Result = mapper.Map<CouponDTO>(coupon); //This is type of CoupunDTO
            }
            catch (Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
            }
            return responseDTO;
        }

		[HttpPost("CreateCoupon")]
        public ResponseDTO CreateCoupon([FromBody]CouponDTO couponDTO)
        {
            try
            {
				Coupon coupon = mapper.Map<Coupon>(couponDTO);
				appDbContext.Coupons.Add(coupon); 
				appDbContext.SaveChanges();

				//why returning couponDTO??
				responseDTO.Result = mapper.Map<CouponDTO>(coupon); ;
				
            }
            catch (Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
            }
            return responseDTO;
        }

        [HttpPut("UpdateCoupon")]
        
        public ResponseDTO UpdateCoupon([FromBody] CouponDTO couponDTO)
        {
            try
            {
                Coupon coupon = mapper.Map<Coupon>(couponDTO);
                appDbContext.Coupons.Update(coupon);
                appDbContext.SaveChanges();

                //why returning couponDTO??
                responseDTO.Result = mapper.Map<CouponDTO>(coupon); ;

            }
            catch (Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
            }
            return responseDTO;
        }

        [HttpDelete]
        [Route("DeleteCoupon/{id}")]
        public ResponseDTO DeleteCoupon(int id)
        {
            try
            {
                Coupon coupon = appDbContext.Coupons.First(x => x.Id == id);
                appDbContext.Coupons.Remove(coupon);
                appDbContext.SaveChanges();


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
