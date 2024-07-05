using Microservices.Web.Interfaces;
using Microservices.Web.Models;
using Microservices.Web.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Microservices.Web.Controllers
{
    public class AuthAPIController : Controller
    
    {
        private readonly IAuthService authService;
        protected ResponseDTO responseDTO;

        public AuthAPIController(IAuthService authService)
        {
            this.authService = authService;
        }
        [HttpGet]
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>() {
                new SelectListItem{Text=Common.RoleAdmin, Value=Common.RoleAdmin},
                new SelectListItem{Text=Common.RoleCustomer, Value=Common.RoleCustomer},

            };
            ViewBag.RoleList = roleList;
            return View();
        }

        [HttpPost]
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
                    return PartialView("_Notification");
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequestDTO)
        {
            if (ModelState.IsValid)
            {
                responseDTO = await authService.Login(loginRequestDTO);
                if (responseDTO.IsSuccess)
                {
                    var json = JsonConvert.SerializeObject(responseDTO.Result);
                    var loginResponseDTO = JsonConvert.DeserializeObject<LoginResponseDTO>(json);

                    //The token can be stored in either cookie or session. Here it is stored in cookie

                    //TempData["success"] = "Login successful!";
                    //return PartialView("_Notification");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    //TempData["error"] = responseDTO.Message;
                    //return PartialView("_Notification");
                    ModelState.AddModelError("CustomeError", responseDTO.Message);
                    return View(loginRequestDTO);
                }
            }
            
            return View();
        }
    }
}
