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
                responseDTO=await couponService.GetCoupons();
                IEnumerable<CouponDTO> couponDTOs;
                if (responseDTO.IsSuccess)
                {
                    var json = JsonConvert.SerializeObject(responseDTO.Result);
                    couponDTOs = JsonConvert.DeserializeObject<IEnumerable<CouponDTO>>(json);
                    return View(couponDTOs);
                }
                else
                {
                    return View("Error");
                }
                
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
                responseDTO = await couponService.GetCoupon(id);
                CouponDTO couponDTO;
                if (responseDTO.IsSuccess)
                {
                    var json = JsonConvert.SerializeObject(responseDTO.Result);
                    couponDTO = JsonConvert.DeserializeObject<CouponDTO>(json);
                    return PartialView(couponDTO);// View(couponDTO);
                }
                else
                {
                    return View("Error");
                }

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
                    responseDTO = await couponService.CreateCoupon(couponDTO);
                    if (responseDTO.IsSuccess)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                        return View("Error");
                }

                catch (Exception ex)
                {
                    return View("Error");
                }

                
            }
            return View("Error");

        }

        [HttpGet]
        public async Task<IActionResult> EditCoupon(int id)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                responseDTO = await couponService.GetCoupon(id);
                if (responseDTO.IsSuccess)
                {
                    CouponDTO couponDTO = new CouponDTO();
                    var json = JsonConvert.SerializeObject(responseDTO.Result);
                    couponDTO = JsonConvert.DeserializeObject<CouponDTO>(json);
                    return PartialView(couponDTO);
                }
                else
                {
                    return View("Error");
                }

            }
            catch(Exception ex)
            {
                return View("Error");
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
                    responseDTO = await couponService.UpdateCoupon(couponDTO);
                    if (responseDTO.IsSuccess)
                    {
                        return new RedirectToActionResult("Index", "Coupon", null);
                    }
                    else
                        return View("Error");

                }
                catch (Exception ex)
                {
                    return View("Error");
                }
            }
            return View("Error");
        }

        //[HttpDelete]

        public async Task<IActionResult> DeleteCoupon(int id)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                responseDTO = await couponService.DeleteCoupon(id);
                if (responseDTO.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
    }
}
