using Microsoft.AspNetCore.Mvc;
using Core.Models;
using Core.IServices;
using Core.DTO_s;
using Core.Models;

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
        public async Task<List<ProductModel>> GetAllProducts()
        {
            return await _productService.GetAllProducts();
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("incorrect product ID");
            }

            ProductModel product = await _productService.GetProductById(id);

            if (product == null)
            {
                return NotFound("product kan niet gevonden worden");
            }

            return Ok(product);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDTO updatedProduct)
        {
            if (id <= 0)
            {
                return BadRequest("Incorrect product ID.");
            }

            if (updatedProduct == null)
            {
                return BadRequest("Updated product data is required.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ProductModel updatedProductModel = await _productService.UpdateProduct(id, updatedProduct);

            if (updatedProductModel == null)
            {
                return NotFound("Product not found.");
            }

            return Ok(updatedProductModel);
        }
    }
}
