using Microsoft.EntityFrameworkCore;
using Core.Models;
using DAL;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Core.IRepositories;

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
            ProductModel existingProduct = await _context.Product.FindAsync(updatedProduct.Id);

            if (existingProduct == null)
            {
                throw new Exception("Product not found.");
            }

            await _context.SaveChangesAsync();

            return existingProduct;
        }
    }
}
