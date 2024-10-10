using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_Shop_API.Models;

namespace Logic.IServices
{
    public interface IProductService
    {
        public Task<List<ProductModel>> GetAllProducts();
    }
}
