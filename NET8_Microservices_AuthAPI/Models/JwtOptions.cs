namespace NET8_Microservices_AuthAPI.Models
{
    public class JwtOptions
    { // This class is used to get the values of JwtOptions from appsettings. It is configured
      // in Program.cs though dependecy injection.
      // In this way we can map to the class entirely. No need to get values of each property.

        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
