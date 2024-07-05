using static Microservices.Web.Utilities.Common;

namespace Microservices.Web.Models
{
    public class RequestDTO
    {
        public APIType APIType { get; set; }
        public object? Data { get; set; }

        public string RequestUri { get; set; }

        public string AccessToken { get; set; }

        public string HttpClientName { get; set; }
    }
}
