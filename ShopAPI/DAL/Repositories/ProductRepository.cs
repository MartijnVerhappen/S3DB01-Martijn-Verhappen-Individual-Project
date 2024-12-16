using DAL.Entities;
using DAL.Mapping;
using Logic.CustomExceptions;
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
            List<ProductEntity> productEntityList = await _dbContext.Product.ToListAsync();
            ICollection<Product> products = ProductMapping.MapTo(productEntityList);
            return products.ToList();
        }

        public async Task<Product> GetProductById(int id)
        {
            ProductEntity productEntity = await _dbContext.Product.FindAsync(id);
            Product product = ProductMapping.MapTo(productEntity);
            return product;
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            ProductEntity existingProduct = await _dbContext.Product.FindAsync(product.Id);
            if (existingProduct == null)
            {
                return null;
            }

            existingProduct = ProductMapping.MapTo(product, existingProduct);

            await _dbContext.SaveChangesAsync();
            
            return ProductMapping.MapTo(existingProduct);
        }

        public async Task<Product> AddProduct(Product product)
        {
            ProductEntity mappedProduct = ProductMapping.MapTo(product, new ProductEntity());
            _dbContext.Product.Add(mappedProduct);
            await _dbContext.SaveChangesAsync();
            return product;
        }
    }
}
