using Web_Shop_API.Models;

namespace Web_Shop_API.IRepositories
{
    public interface IProductRepository
    {
        public Task<List<ProductModel>> GetAllProducts();

        public Task<ProductModel> GetProductById(int id);
    }
}
