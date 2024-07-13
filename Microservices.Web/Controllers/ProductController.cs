using Microservices.Web.Interfaces;
using Microservices.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Microservices.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService productService;
        private ResponseDTO responseDTO;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<ProductDTO> productDTOs = new();
            responseDTO = await productService.GetProductsAsync();
            if(responseDTO.IsSuccess)
            {
                var json = JsonConvert.SerializeObject(responseDTO.Result);
                 productDTOs=JsonConvert.DeserializeObject<List<ProductDTO>>(json);
            }
            else
            {
                TempData["error"] = responseDTO.Message;
            }
            return View(productDTOs);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
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

        
        public async Task<IActionResult> Delete(int id)
        {
            ProductDTO productDTO = new();
            responseDTO = await productService.DeleteProductAsync(id);
            if (!responseDTO.IsSuccess)
                TempData["error"] = responseDTO.Message;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductDTO productDTO)
        {
            if (ModelState.IsValid)
            {
                responseDTO = await productService.CreateProductAsync(productDTO);
                if (responseDTO.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["error"] = responseDTO.Message;
                }
            }
            return View(productDTO);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
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

        [HttpPost]
        public async Task<IActionResult> Update(ProductDTO productDTO)
        {
            if (ModelState.IsValid)
            {
                responseDTO = await productService.UpdateProductAsync(productDTO);
                if (responseDTO.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["error"] = responseDTO.Message;
                }
            }
            return View(productDTO);
        }
    }
}
