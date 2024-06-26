using Microsoft.AspNetCore.Identity;

namespace NET8_Microservices_AuthAPI.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Name { get; set; }
    }
}
