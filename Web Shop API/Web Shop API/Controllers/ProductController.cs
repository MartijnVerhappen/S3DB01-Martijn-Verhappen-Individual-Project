using Microsoft.AspNetCore.Mvc;
using Web_Shop_API.Models;
using Logic.IServices;

namespace Web_Shop_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet(Name = "GetAllProducts")]
        public Task<List<ProductModel>> GetAllProducts()
        {
            return _productService.GetAllProducts();
        }
    }
}
