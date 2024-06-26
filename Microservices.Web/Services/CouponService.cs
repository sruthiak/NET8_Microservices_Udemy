using Microservices.Web.Interfaces;
using Microservices.Web.Models;
using Microservices.Web.Utilities;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Microservices.Web.Services
{
    public class CouponService:ICouponService
    {
        //private readonly IHttpClientFactory httpClientFactory;
        private readonly IBaseService baseService;


        public CouponService(IBaseService baseService)
        {
            this.baseService = baseService;

        }

        public async Task<ResponseDTO> CreateCouponAsync(CouponDTO couponDTO)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            RequestDTO requestDTO = new RequestDTO();
            try
            {
                requestDTO.Data = couponDTO;
                requestDTO.RequestUri=@"api/CouponAPI/CreateCoupon";
                requestDTO.APIType = Common.APIType.POST;
                
                responseDTO= await baseService.SendAsync(requestDTO);
                
            }
            catch(Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> GetCouponAsync(int id)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            //RequestDTO requestDTO = new RequestDTO();
            try
            {
                //requestDTO.Data = null;
                //requestDTO.RequestUri= $"api/CouponAPI/{id.ToString()}";
                //requestDTO.APIType = Common.APIType.GET;
                responseDTO = await baseService.SendAsync(new RequestDTO
                {
                    Data=null,
                    RequestUri= $"api/CouponAPI/{id.ToString()}",
                    APIType=Common.APIType.GET
                });
                
            }
            catch(Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> DeleteCouponAsync(int id)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            RequestDTO requestDTO = new RequestDTO();
            try
            {
                requestDTO.RequestUri = $"api/CouponAPI/DeleteCoupon/{id}";
                requestDTO.APIType = Common.APIType.DELETE;
                requestDTO.Data = null;
                responseDTO = await baseService.SendAsync(requestDTO);
                

            }
            catch (Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
            }
            return responseDTO;
        }



        public async Task<ResponseDTO> GetCouponsAsync()
        {
            ResponseDTO responseDTO = new ResponseDTO();
            RequestDTO requestDTO = new RequestDTO();
            try
            {

                requestDTO.APIType = Common.APIType.GET;
                requestDTO.RequestUri = @"api/CouponAPI11";

                responseDTO = await baseService.SendAsync(requestDTO);


            }
            catch (Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
            }

            return responseDTO;
        }


        public async Task<ResponseDTO> UpdateCouponAsync(CouponDTO couponDTO)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            RequestDTO requestDTO = new RequestDTO();
            try
            {
                requestDTO.APIType = Common.APIType.PUT;
                requestDTO.Data = couponDTO;
                requestDTO.RequestUri = @"api/CouponAPI/UpdateCoupon";

                /*
                 When calling a HttpPut method, we need to pass the HttpContent.
                In the CouponAPIController, UpdateCoupon method has a parameter attribut [FromBody]
                So will serialize couponDTO object and pass the Json string content.
                In the API method [FromBody] will deserialize this Json content to the CouponDTO object
                */

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
