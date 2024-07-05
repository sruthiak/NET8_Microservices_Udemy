using NET8_Microservices_AuthAPI.Models;

namespace NET8_Microservices_AuthAPI.Services.IServices
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser applicationUser);
    }
}
