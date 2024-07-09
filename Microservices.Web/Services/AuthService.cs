using Microservices.Web.Interfaces;
using Microservices.Web.Models;
using Microservices.Web.Utilities;
using Newtonsoft.Json;

namespace Microservices.Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService baseService;
        protected RequestDTO requestDTO;
        protected ResponseDTO responseDTO;

        public AuthService(IBaseService baseService)
        {
            this.baseService = baseService;
        }

        public async Task<ResponseDTO> AssignRole(RegistrationRequestDTO registrationRequestDTO)
        {
            try
            {
                requestDTO = new()
                {
                    Data=registrationRequestDTO,
                    APIType=Common.APIType.POST,
                    RequestUri=@"api/AuthAPI/assignrole",
                    HttpClientName="AuthClient"

                };

                responseDTO = await baseService.SendAsync(requestDTO);
            }
            catch (Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {

            try
            {
                requestDTO = new()
                {
                    Data = loginRequestDTO,
                    RequestUri = @"api/AuthAPI/login",
                    APIType = Common.APIType.POST,
                    HttpClientName = "AuthClient",
                };

                responseDTO = await baseService.SendAsync(requestDTO,withBearer:false);
                

            }
            catch(Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
            }

            return responseDTO;
        }

        public async Task<ResponseDTO> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            
            try
            {
                requestDTO = new()
                {
                    Data = registrationRequestDTO,
                    RequestUri = $"api/authapi/register",
                    APIType = Common.APIType.POST,
                    HttpClientName= "AuthClient"
                };
                responseDTO =await baseService.SendAsync(requestDTO, withBearer: false);
                

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
