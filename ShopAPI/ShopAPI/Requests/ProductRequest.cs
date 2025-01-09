using Logic.Models;

namespace ShopAPI.Requests
{
    public class ProductRequest
    {
        public Product product { get; set; }
        public int aantal { get; set; }
    }
}
