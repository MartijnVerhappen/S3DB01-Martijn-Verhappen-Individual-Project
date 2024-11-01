using Microsoft.AspNetCore.Mvc;
using Web_Shop_API.Models;

namespace Web_Shop_API.DAL.IRepositories
{
    public interface IProductRepository
    {
        public Task<List<ProductModel>> GetAllProducts();

        public Task<ProductModel> GetProductById(int id);

        public Task<ProductModel> UpdateProduct(ProductModel updatedProduct);
    }
}
