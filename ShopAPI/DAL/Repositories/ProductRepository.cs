using Logic.IRepositories;
using Logic.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DBContext _dbContext;

        public ProductRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            return await _dbContext.Product.ToListAsync();
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _dbContext.Product.FindAsync(id);
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            var existingProduct = await _dbContext.Product.FindAsync(product.Id);
            if (existingProduct == null)
            {
                return null;
            }

            existingProduct.ProductType = product.ProductType;
            existingProduct.ProductNaam = product.ProductNaam;
            existingProduct.ProductPrijs = product.ProductPrijs;
            existingProduct.ProductKorting = product.ProductKorting;

            await _dbContext.SaveChangesAsync();
            return existingProduct;
        }

        public async Task<Product> AddProduct(Product product)
        {
            _dbContext.Product.Add(product);
            await _dbContext.SaveChangesAsync();
            return product;
        }
    }
}
