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
        private readonly ITokenProvider tokenProvider;

        public BaseService(IHttpClientFactory httpClientFactory,ITokenProvider tokenProvider)
        {
            this.httpClientFactory = httpClientFactory;
            this.tokenProvider = tokenProvider;
        }
        public async Task<ResponseDTO> SendAsync(RequestDTO requestDTO,bool withBearer)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            
            try
            {
                var client = httpClientFactory.CreateClient(requestDTO.HttpClientName);
                HttpRequestMessage httpRequestMessage = new();

                httpRequestMessage.Headers.Add("Accept", "application/json");

                //we need to pass token to CouponAPI and all other APIs because they use [Authorize]
                //if not passed, then will get UnAuthorized error when clicked the Coupon link in browser
                if (withBearer)
                {
                    var token = tokenProvider.GetToken();// Token is stored in cookie in the Login method
                    httpRequestMessage.Headers.Add("Authorization", $"Bearer {token}");
                }
                
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
