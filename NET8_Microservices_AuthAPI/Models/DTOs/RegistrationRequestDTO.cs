﻿namespace NET8_Microservices_AuthAPI.Models.DTOs
{
    public class RegistrationRequestDTO
    { 
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string Password { get; set; }
        public string Role { get; set; }
    }
}
