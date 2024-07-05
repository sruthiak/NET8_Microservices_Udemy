using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NET8_Microservices_AuthAPI.Models.DTOs;
using NET8_Microservices_AuthAPI.Services.IServices;
using NET8_Microservices_Udemy.Services.AuthAPI.Models.DTOs;

namespace NET8_Microservices_AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService authService;
        protected ResponseDTO responseDTO;

        public AuthAPIController(IAuthService authService)
        {
            this.authService = authService;
            responseDTO = new();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegistrationRequestDTO registrationRequestDTO)
        {
            var errorMessage= await authService.Registration(registrationRequestDTO);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = errorMessage;
                return BadRequest(responseDTO);
            }
            return Ok(responseDTO);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginRequestDTO loginRequestDTO)
        {
            var loginResponseDTO = await authService.Login(loginRequestDTO);
            if(loginResponseDTO.User==null)
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = "Username or Password is invalid";
                return BadRequest(responseDTO);

            }
            responseDTO.Result = loginResponseDTO;
            return Ok(responseDTO);
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDTO registrationRequestDTO)
        {
            if (!await authService.AssignRole(registrationRequestDTO.Email, registrationRequestDTO.Role.ToUpper()))
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = "Error - Cannot assign role";
                return BadRequest(responseDTO);
            }
            return Ok(responseDTO);
        }
    }
}
