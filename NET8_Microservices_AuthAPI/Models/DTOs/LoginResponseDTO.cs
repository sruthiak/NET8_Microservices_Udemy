namespace NET8_Microservices_AuthAPI.Models.DTOs
{
    public class LoginResponseDTO
    {
        //This is the UserDTO sent when Registration is successful
        public UserDTO User { get; set; }

        //This is the Jwt token used to validate the user that is logged in.
        //Jwt is configured in appsettings
        public string Token { get; set; }
    }
}
