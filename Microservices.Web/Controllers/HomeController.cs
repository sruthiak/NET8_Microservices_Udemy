using Microservices.Web.Interfaces;
using Microservices.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Microservices.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService productService;
        private ResponseDTO responseDTO;

        public HomeController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<ProductDTO> productDTOs = new();
            responseDTO = await productService.GetProductsAsync();
            if (responseDTO.IsSuccess)
            {
                var json = JsonConvert.SerializeObject(responseDTO.Result);
                productDTOs = JsonConvert.DeserializeObject<List<ProductDTO>>(json);
            }
            else
            {
                TempData["error"] = responseDTO.Message;
            }
            return View(productDTOs);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ProductDetails(int id)
        {
            ProductDTO productDTO = new();
            responseDTO = await productService.GetProductByIdAsync(id);
            if (responseDTO.IsSuccess)
            {
                var json = JsonConvert.SerializeObject(responseDTO.Result);
                productDTO = JsonConvert.DeserializeObject<ProductDTO>(json);
            }
            else
            {
                TempData["error"] = responseDTO.Message;
            }
            return View(productDTO);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
