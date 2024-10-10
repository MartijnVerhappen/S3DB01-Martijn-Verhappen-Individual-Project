using Web_Shop_API.Entities;

namespace Web_Shop_API.IRepositories
{
    public interface IProductRepository
    {
        public List<ProductEntity> GetAllProducts();

        public ProductEntity GetProductById(int id);
    }
}
