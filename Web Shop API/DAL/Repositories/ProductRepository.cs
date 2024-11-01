using Microsoft.EntityFrameworkCore;
using Web_Shop_API.Models;
using DAL;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Web_Shop_API.DAL.IRepositories;

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


        public async Task<ProductModel> GetProductById(int id)
        {
            return await _context.Product.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<ProductModel> UpdateProduct(ProductModel updatedProduct)
        {
            var existingProduct = await _context.Product.FindAsync(updatedProduct.Id);

            if (existingProduct == null)
            {
                throw new Exception("Product not found.");
            }

            existingProduct.ApplyChanges(updatedProduct);

            // Save changes to the database
            await _context.SaveChangesAsync();

            return existingProduct;
        }
    }
}
