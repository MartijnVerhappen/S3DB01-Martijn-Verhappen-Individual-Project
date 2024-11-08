using Core.Models;
using Core.DTO_s;

namespace Core.IServices
{
    public interface IProductService
    {
        Task<List<ProductModel>> GetAllProducts();
        Task<ProductModel> GetProductById(int id);
        Task<ProductModel> UpdateProduct(int id, ProductDTO updatedProductDto);
    }
}
