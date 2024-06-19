using Microservices.Web.Interfaces;
using Microservices.Web.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Microservices.Web.Services
{
    public class CouponService:ICouponService
    {
        private readonly IHttpClientFactory httpClientFactory;
        

        public CouponService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
            
        }

        public async Task<ResponseDTO> CreateCoupon(CouponDTO couponDTO)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                var client = httpClientFactory.CreateClient("CouponClient");
                // [FromBody] attribute in the API Create method will automatically deserialise json string to Coupon object
                var json = JsonConvert.SerializeObject(couponDTO);
                var content=new StringContent(json, Encoding.UTF8, "application/json"); 
                var httpResponseMessage = await client.PostAsync($"{client.BaseAddress}api/CouponAPI/CreateCoupon",content);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var result = httpResponseMessage.Content.ReadAsStringAsync().Result;
                    responseDTO = JsonConvert.DeserializeObject<ResponseDTO>(result);
                }
                else
                {
                    responseDTO.IsSuccess = false;
                    responseDTO.Message = httpResponseMessage.ReasonPhrase;
                }
            }
            catch(Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> DeleteCoupon(int id)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                var client=httpClientFactory.CreateClient("CouponClient");
                var httpResponseMessage =await client.DeleteAsync($"{client.BaseAddress}api/CouponAPI/DeleteCoupon/{id}");
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var result =httpResponseMessage.Content.ReadAsStringAsync().Result;
                    //var json = JsonConvert.SerializeObject(result);
                    responseDTO = JsonConvert.DeserializeObject<ResponseDTO>(result);
                   
                }
                else
                {
                    responseDTO.IsSuccess = false;
                    responseDTO.Message = httpResponseMessage.ReasonPhrase;
                }


            }
            catch(Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> GetCoupon(int id)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {

                var client = httpClientFactory.CreateClient("CouponClient"); 
                HttpResponseMessage httpResponseMessage = await client.GetAsync($"{client.BaseAddress}api/CouponAPI/{id}");

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var result = httpResponseMessage.Content.ReadAsStringAsync().Result;
                    responseDTO = JsonConvert.DeserializeObject<ResponseDTO>(result);


                    
                }
                else
                {
                    responseDTO.IsSuccess = false;
                    responseDTO.Message = httpResponseMessage.ReasonPhrase;
                }

            }
            catch (Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
            }

            return responseDTO;
        }

        public async Task<ResponseDTO> GetCoupons()
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                
                var client = httpClientFactory.CreateClient("CouponClient");
                
                HttpResponseMessage httpResponseMessage =await client.GetAsync(client.BaseAddress+"api/CouponAPI");
                
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var result = httpResponseMessage.Content.ReadAsStringAsync().Result;
                    responseDTO= JsonConvert.DeserializeObject <ResponseDTO>(result);
                    
                    
;               }
                else
                {
                    responseDTO.IsSuccess = false;
                    responseDTO.Message = httpResponseMessage.ReasonPhrase;
                }

            }
            catch(Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
            }

            return responseDTO;
        }

        public async Task<ResponseDTO> UpdateCoupon(CouponDTO couponDTO)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                var client = httpClientFactory.CreateClient("CouponClient");

                /*
                 When calling a HttpPut method, we need to pass the HttpContent.
                In the CouponAPIController, UpdateCoupon method has a parameter attribut [FromBody]
                So will serialize couponDTO object and pass the Json string content.
                In the API method [FromBody] will deserialize this Json content to the CouponDTO object
                */

                //serialise to json string
                var json = JsonConvert.SerializeObject(couponDTO);
                //convert to HttpContent- pass json string, encoding and mediatype
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var httpResponseMessage = await client.PutAsync($"{client.BaseAddress}api/CouponAPI/UpdateCoupon", content);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var result= httpResponseMessage.Content.ReadAsStringAsync().Result;
                    responseDTO = JsonConvert.DeserializeObject<ResponseDTO>(result);
                    
                }
                else
                {
                    responseDTO.IsSuccess = false;
                    responseDTO.Message = httpResponseMessage.ReasonPhrase;
                }
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
