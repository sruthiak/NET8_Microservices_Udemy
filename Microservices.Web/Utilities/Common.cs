namespace Microservices.Web.Utilities
{
    public class Common
    {
        public static string RequestUri { get; set; } = string.Empty;

        public const string RoleAdmin = "Admin";
        public const string RoleCustomer = "Customer";

        public const string TokenCookie = "JwtToken";
        public enum APIType
        {
            GET,
            POST,
            PUT, 
            DELETE
        }
    }
}
