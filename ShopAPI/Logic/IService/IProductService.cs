using Logic.Models;

namespace Logic.IService
{
    public interface IProductService
    {
        public Task<List<Product>> GetAllProducts();

        public Task<Product> GetProductById(int id);

        public Task<Product> UpdateProduct(Product product);

        public Task<Product> AddProduct(Product product);
    }
}
