using Microservices.Web.Interfaces;
using Microservices.Web.Models;
using Microservices.Web.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Microservices.Web.Controllers
{
    [Route("{controller}")]
    public class AuthAPIController : Controller
    
    {
        private readonly IAuthService authService;
        private readonly ITokenProvider tokenProvider;
        protected ResponseDTO responseDTO;

        public AuthAPIController(IAuthService authService,ITokenProvider tokenProvider)
        {
            this.authService = authService;
            this.tokenProvider = tokenProvider;
        }
        [HttpGet("Register")]
       
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>() {
                new SelectListItem{Text=Common.RoleAdmin, Value=Common.RoleAdmin},
                new SelectListItem{Text=Common.RoleCustomer, Value=Common.RoleCustomer},

            };
            ViewBag.RoleList = roleList;
            return View();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            if (ModelState.IsValid)
            {
                responseDTO = await authService.Register(registrationRequestDTO);
                if (responseDTO.IsSuccess && responseDTO!=null)
                {
                    //call assignrole
                    if (string.IsNullOrEmpty(registrationRequestDTO.Role))
                    {
                        //assign customer role
                        registrationRequestDTO.Role = Common.RoleCustomer;
                    }
                    ResponseDTO assignRoleResponse = await authService.AssignRole(registrationRequestDTO);
                    if(assignRoleResponse!=null && assignRoleResponse.IsSuccess)
                    {
                        TempData["success"] = "Registration successful!";
                        return RedirectToAction(nameof(Login));

                    }
                    
                }
                else
                {
                    TempData["error"] = responseDTO.Message;
                    //return PartialView("_Notification"); this is not required, just return current view
                }
            }
            return View();
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequestDTO)
        {
            if (ModelState.IsValid)
            {
                responseDTO = await authService.Login(loginRequestDTO);
                if (responseDTO.IsSuccess)
                {
                    var json = JsonConvert.SerializeObject(responseDTO.Result);
                    var loginResponseDTO = JsonConvert.DeserializeObject<LoginResponseDTO>(json);


                    //The token can be stored in either cookie or session. Here it is stored in cookie.
                    //Cookie is managed in ITokenProvider
                    // Also need to register cookie authentication in Program.cs
                    if (loginResponseDTO.Token != null)
                    {

                        //sign in user using built in .net identity
                        await SignInUser(loginResponseDTO);

                        //can check cookie in developer tools Application tab -> Cookies
                        //Cookie can be used for authentication (Cookie
                        //AuthenticationDefaults.AuthenticationScheme) OR to store some data -Represents an
                        //individual HTTP cookie within an HttpContext object. Cookies in HttpContext are
                        //used to store and retrieve arbitrary data (not necessarily related to
                        //authentication).
                        tokenProvider.SetToken(loginResponseDTO.Token);
                    }

                    //TempData["success"] = "Login successful!";
                    //return PartialView("_Notification");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["error"] = responseDTO.Message;
                    //return PartialView("_Notification"); not required, just return current view
                    //ModelState.AddModelError("CustomeError", responseDTO.Message);
                    return View(loginRequestDTO);
                }
            }
            
            return View();
        }

        /// <summary>
        /// Sign in User in .NET Identity. See video 60.
        /// In Layout.cshtml we have used @User.Identity.IsAuthenticated. 
        /// So to make the user identity Authenticated we need this
        /// </summary>
        /// <returns></returns>
        private async Task SignInUser(LoginResponseDTO loginResponseDTO)
        {
            //Install the System.IdentityModel.Tokens.Jwt NuGet package.
            //Use JwtSecurityTokenHandler to create and validate JWTs in your.NET Core application.
            //we have used this in AuthAPI service for creating the jwt token
            
            var jwtHandler = new JwtSecurityTokenHandler();

            var jwtToken = jwtHandler.ReadJwtToken(loginResponseDTO.Token);

            //Claim => A Claim represents a single statement about a user. Key,Value pair.
            //example, a claim might state that a user has the email "user@example.com"
            //or belongs to the role "Administrator".

            //ClaimsIdentity => Collection of claims. It is used to represent a user's identity.

            //ClaimsPrincipal => A ClaimsPrincipal can contain multiple ClaimsIdentity objects,
            //representing different sources of identity information (e.g., multiple authentication schemes).
            //It is used to represent a user(principal) with all assocaited Identities(represented by ClaimsIdentity)

            //In layout.cshtml , we used User.Identity.IssAuthenticated.
            //You can access the user’s identity and claims in any controller action using the User property:
            //Here we use cookie-based authentication.
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);


            //create claims similar to JWT registered claim names which we created before.
            //Claim is Key value pair.
            //Check JwtTokenGenerator.cs
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, 
                jwtToken.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
                jwtToken.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
                jwtToken.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));

            //also we need to add Name and Role .net identity claim along with JWtregistered claims
            identity.AddClaim(new Claim(ClaimTypes.Name,
                jwtToken.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));

            //Role is not used in JwtRegisteredClaimNames, instead ClaimTypes.Role is used.
            //This is for .net integration. It automatically takes care of [Authorize(Roles=Common.AdminRole)] etc
            identity.AddClaim(new Claim(ClaimTypes.Role,
                jwtToken.Claims.FirstOrDefault(u => u.Type == "role").Value));


            var claimsPrincipal = new ClaimsPrincipal(identity);

            //this expects an authentication scheme and Claims Principal
            //here we use cookie authentication default. This will sign in user using .net identity
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

        }

        [HttpGet("Logout")]
        public async Task<IActionResult> SignOutUser()
        {
            await HttpContext.SignOutAsync();
            tokenProvider.ClearToken();
            return RedirectToAction("Index", "Home");
        }
    }
}
