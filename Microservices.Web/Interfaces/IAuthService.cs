using Microservices.Web.Models;

namespace Microservices.Web.Interfaces
{
    public interface IAuthService
    {
        Task<ResponseDTO> Register(RegistrationRequestDTO registrationRequestDTO);
        Task<ResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<ResponseDTO> AssignRole(RegistrationRequestDTO registrationRequestDTO);
    }
}
