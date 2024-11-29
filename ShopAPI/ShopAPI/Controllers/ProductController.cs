using Microsoft.AspNetCore.Mvc;
using Logic.IRepositories;
using Logic.IService;
using Logic.Models;

namespace ShopAPI.Controllers
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
        public async Task<List<Product>> GetAllProducts()
        {
            return await _productService.GetAllProducts();
        }

        [HttpGet("{id:int}")]
        public async Task<Product> GetProductById(int id)
        {
            return await _productService.GetProductById(id);
        }

        [HttpPut(Name = "Update")]
        public async Task<Product> UpdateProduct([FromBody]Product product)
        {
             return await _productService.UpdateProduct(product);
        }

        [HttpPost(Name = "Add")]
        public async Task<Product> AddProduct([FromBody]Product product)
        {
            return await _productService.AddProduct(product);
        }
    }
}
