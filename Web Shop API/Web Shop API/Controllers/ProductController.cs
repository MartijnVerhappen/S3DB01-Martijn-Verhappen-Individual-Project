using Microsoft.AspNetCore.Mvc;
using Web_Shop_API.Entities;
using Web_Shop_API.Services;

namespace Web_Shop_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : Controller
    {
        private readonly ProductService _productService;
        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet(Name = "GetAllProducts")]
        public List<ProductEntity> GetAllProducts()
        {
            return _productService.GetAllProducts();
        }
    }
}
