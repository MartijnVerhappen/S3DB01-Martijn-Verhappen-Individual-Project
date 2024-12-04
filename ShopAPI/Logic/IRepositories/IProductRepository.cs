using Logic.Models;

namespace Logic.IRepositories
{
    public interface IProductRepository
    {
        public Task<List<Product>> GetAllProducts();

        public Task<Product> GetProductById(int id);

        public Task<Product> UpdateProduct(Product Product);

        public Task<Product> AddProduct(Product product);
    }
}
