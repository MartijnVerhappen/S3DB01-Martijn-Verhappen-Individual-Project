using Core.Models;

namespace Core.IRepositories
{
    public interface IProductRepository
    {
        public Task<List<ProductModel>> GetAllProducts();

        public Task<ProductModel> GetProductById(int id);

        public Task<ProductModel> UpdateProduct(ProductModel updatedProduct);
    }
}
