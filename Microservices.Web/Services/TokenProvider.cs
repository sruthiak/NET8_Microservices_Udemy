using Microservices.Web.Interfaces;
using Microservices.Web.Utilities;
using NuGet.Common;

namespace Microservices.Web.Services
{
    public class TokenProvider : ITokenProvider
    {
        /// <summary>
        /// the HttpContextAccessor class is a component that provides access to the 
        /// current HTTP request and response context. It allows you to access various aspects
        /// of the HTTP request and response, such as headers, cookies, query parameters, and user claims.
        /// https://www.c-sharpcorner.com/article/httpcontextaccessor-in-asp-net-core-web-api/ 
        /// </summary>
        private readonly IHttpContextAccessor httpContextAccessor;

        public TokenProvider(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
        public void ClearToken()
        {
            httpContextAccessor.HttpContext?.Response.Cookies.Delete(Common.TokenCookie);
        }

        public string? GetToken()
        {
            string? token=null;
            //This time we are requesting a cookie. So check the Request of httpContext
            bool? hasCookie= httpContextAccessor.HttpContext?.Request.Cookies.TryGetValue(Common.TokenCookie, out token);

            return hasCookie is true ? token : null;
        }

        public void SetToken(string token)
        {
            httpContextAccessor.HttpContext?.Response.Cookies.Append(Common.TokenCookie, token);

        }
    }
}
