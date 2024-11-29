using Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
