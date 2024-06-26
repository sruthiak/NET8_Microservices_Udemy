using Microservices.Web.Interfaces;
using Microservices.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Microservices.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly ICouponService couponService;

        public CouponController(IConfiguration configuration,ICouponService couponService)
        {
            this.configuration = configuration;
            this.couponService = couponService;
        }
        public async Task<IActionResult> Index()
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                responseDTO=await couponService.GetCouponsAsync();
                List<CouponDTO> couponDTOs=new();
                if (responseDTO.IsSuccess)
                {
                    var json = JsonConvert.SerializeObject(responseDTO.Result);
                    couponDTOs = JsonConvert.DeserializeObject<List<CouponDTO>>(json);
                    
                }
                else
                {
                    TempData["error"] = responseDTO.Message;
                    //return PartialView("_Notification");
                }
                return View(couponDTOs);

            }
            catch(Exception ex)
            {
                return View("Error");
            }
        }

        public async Task<IActionResult> GetCoupon(int id)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                responseDTO = await couponService.GetCouponAsync(id);
                CouponDTO couponDTO=new CouponDTO();
                if (responseDTO.IsSuccess)
                {
                    var json = JsonConvert.SerializeObject(responseDTO.Result);
                    couponDTO = JsonConvert.DeserializeObject<CouponDTO>(json);
                    return View(couponDTO);
                }
                else
                {
                    TempData["error"] = responseDTO.Message;
                    //return PartialView("_Notification");
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return View("Error");
            }
            
        }

        [HttpGet]
        public async Task<IActionResult> CreateCoupon()
        {
            return PartialView();
        }
        [HttpPost]
        public async Task<IActionResult> CreateCoupon(CouponDTO couponDTO)
        {
            if (ModelState.IsValid)
            {
                ResponseDTO responseDTO = new ResponseDTO();
                try
                {
                    responseDTO = await couponService.CreateCouponAsync(couponDTO);
                    if (responseDTO.IsSuccess)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["error"] = responseDTO.Message;
                        return PartialView("_Notification");
                    }
                }

                catch (Exception ex)
                {
                    TempData["error"] = responseDTO.Message;
                    return PartialView("_Notification");
                }

                
            }
            return View();
            
        }

        [HttpGet]
        public async Task<IActionResult> EditCoupon(int id)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                responseDTO = await couponService.GetCouponAsync(id);
                if (responseDTO.IsSuccess)
                {
                    CouponDTO couponDTO = new CouponDTO();
                    var json = JsonConvert.SerializeObject(responseDTO.Result);
                    couponDTO = JsonConvert.DeserializeObject<CouponDTO>(json);
                    return PartialView(couponDTO);
                }
                else
                {
                    TempData["error"] = responseDTO.Message;
                    return PartialView("_Notification");
                }

            }
            catch (Exception ex)
            {
                TempData["error"] = responseDTO.Message;
                return PartialView("_Notification");
            }

        }

        [HttpPost]
        public async Task<IActionResult> EditCoupon(CouponDTO couponDTO)
        {
            if (ModelState.IsValid)
            {
                ResponseDTO responseDTO = new ResponseDTO();
                try
                {
                    responseDTO = await couponService.UpdateCouponAsync(couponDTO);
                    if (responseDTO.IsSuccess)
                    {
                        return new RedirectToActionResult("Index", "Coupon", null);
                    }
                    else
                    {
                        TempData["error"] = responseDTO.Message;
                        return PartialView("_Notification");
                    }

                }
                catch (Exception ex)
                {
                    TempData["error"] = responseDTO.Message;
                    return PartialView("_Notification");
                }
            }
            return View();
        }

       // [HttpDelete] - This is not HttpDelete.  It wont work if mentioned HttpDelete.
       // It is in CouponAPIController which actually deletes.

        public async Task<IActionResult> DeleteCoupon(int id)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                responseDTO = await couponService.DeleteCouponAsync(id);
                if (responseDTO.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["error"] = responseDTO.Message;
                    return PartialView("_Notification");
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = responseDTO.Message;
                return PartialView("_Notification");
            }
        }
    }
}
