using Microsoft.EntityFrameworkCore;
using Web_Shop_API.IRepositories;
using Web_Shop_API.Models;
using DAL;

namespace Web_Shop_API.Repositories
{
    
    public class ProductRepository : IProductRepository
    {
        private readonly DBContext _context;

        public ProductRepository(DBContext context) 
        {
            _context = context;
        }

        public async Task<List<ProductModel>> GetAllProducts()
        {
            return await _context.Product.ToListAsync();
        }


        public Task<ProductModel> GetProductById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
