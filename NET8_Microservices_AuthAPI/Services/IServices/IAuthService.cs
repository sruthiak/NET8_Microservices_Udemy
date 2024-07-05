using NET8_Microservices_AuthAPI.Models.DTOs;

namespace NET8_Microservices_AuthAPI.Services.IServices
{
    public interface IAuthService
    {
        Task<string> Registration(RegistrationRequestDTO registrationRequestDTO);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);

        Task<bool> AssignRole(string email, string roleName);
    }
}
