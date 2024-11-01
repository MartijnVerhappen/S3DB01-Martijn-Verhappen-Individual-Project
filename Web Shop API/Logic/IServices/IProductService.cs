using Web_Shop_API.Models;

namespace Logic.IServices
{
    public interface IProductService
    {
        Task<List<ProductModel>> GetAllProducts();
        Task<ProductModel> GetProductById(int id);

        Task<ProductModel> UpdateProduct(ProductModel updatedProduct);
    }
}
