using Microsoft.AspNetCore.Mvc;

namespace Web_Shop_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : Controller
    {
        {
            _productService = productService;
        }

        [HttpGet(Name = "GetAllProducts")]
        {
            return _productService.GetAllProducts();
        }
    }
}
