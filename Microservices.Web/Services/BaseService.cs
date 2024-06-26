using Microservices.Web.Interfaces;
using Microservices.Web.Models;
using Microservices.Web.Utilities;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using static Microservices.Web.Utilities.Common;

namespace Microservices.Web.Services
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory httpClientFactory;

        public BaseService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<ResponseDTO> SendAsync(RequestDTO requestDTO)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            
            try
            {
                var client = httpClientFactory.CreateClient("CouponClient");
                HttpRequestMessage httpRequestMessage = new();
                httpRequestMessage.RequestUri = new Uri($"{Common.RequestUri}{requestDTO.RequestUri}");                

                switch (requestDTO.APIType)
                {
                    case APIType.POST: httpRequestMessage.Method =HttpMethod.Post ;break;
                    case APIType.PUT: httpRequestMessage.Method = HttpMethod.Put; break;
                    case APIType.DELETE: httpRequestMessage.Method = HttpMethod.Delete; break;
                    default: httpRequestMessage.Method = HttpMethod.Get; break;

                }

                if (requestDTO.Data != null)
                {
                    // [FromBody] attribute in the API Create method will automatically deserialise json string to Coupon object
                    var json = JsonConvert.SerializeObject(requestDTO.Data);
                    httpRequestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");
                }


                var httpResponseMessage = await client.SendAsync(httpRequestMessage);
                if(httpResponseMessage != null && httpResponseMessage.IsSuccessStatusCode)
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
    }
}
