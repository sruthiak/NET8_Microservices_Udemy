namespace NET8_Microservices_AuthAPI.Models.DTOs
{
    //'ApplicationUser' model class contains many information. So we need a DTO
    //This is passed when Login successful along with token
    public class UserDTO
    {
        public string  Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
