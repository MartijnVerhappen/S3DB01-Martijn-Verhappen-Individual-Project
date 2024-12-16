using DAL.Entities;
using Logic.Models;

namespace DAL.Mapping
{
    public class ProductMapping
    {
        public static ICollection<Product> MapTo(ICollection<ProductEntity> products)
        {
            List<Product> productList = new List<Product>();
            foreach (ProductEntity product in products) 
            {
                productList.Add(MapTo(product));
            }
            return productList;
        }
        public static ICollection<ProductEntity> MapTo(ICollection<Product> products)
        {
            List<ProductEntity> productEntityList = new List<ProductEntity>();
            foreach (Product product in products)
            {
                productEntityList.Add(MapTo(product, new ProductEntity()));
            }
            return productEntityList;
        }

        public static Product MapTo(ProductEntity productEntity) 
        {
            Product product = new Product
            {
                Id = productEntity.Id,
                ProductType = productEntity.ProductType,
                ProductNaam = productEntity.ProductNaam,
                ProductPrijs = productEntity.ProductPrijs,
                ProductKorting = productEntity.ProductKorting
            };
            return product;
        }

        public static ProductEntity MapTo(Product product, ProductEntity productEntity)
        {
            productEntity.Id = product.Id;
            productEntity.ProductType = product.ProductType;
            productEntity.ProductNaam = product.ProductNaam;
            productEntity.ProductPrijs = product.ProductPrijs;
            productEntity.ProductKorting = product.ProductKorting;
            return productEntity;
        }
    }
}