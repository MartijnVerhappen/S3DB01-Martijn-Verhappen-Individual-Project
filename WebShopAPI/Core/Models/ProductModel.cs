using System.ComponentModel.DataAnnotations;
using Core.DTO_s;

namespace Core.Models
{
    public class ProductModel
    {
        public int Id { get; private set; }

        public string ProductType { get; private set; }

        public string ProductNaam { get; private set; }

        public double ProductPrijs { get; private set; }

        public int ProductKorting { get; private set; }

        public ProductModel() { }

        public ProductModel(int id, string productType, string productnaam, double productPrijs, int productKorting) 
        { 
            Id = id;
            ProductType = productType;
            ProductNaam = productnaam;
            ProductPrijs = productPrijs;
            ProductKorting = productKorting;
        }

        public void Update(string productType, string productNaam, double productPrijs, int productKorting)
        {
            ProductType = productType;
            ProductNaam = productNaam;
            ProductPrijs = productPrijs;
            ProductKorting = productKorting;
        }

        public void ApplyChanges(ProductDTO updatedProduct)
        {
            if (!string.IsNullOrEmpty(updatedProduct.ProductType))
            {
                ProductType = updatedProduct.ProductType;
            }

            if (!string.IsNullOrEmpty(updatedProduct.ProductNaam))
            {
                ProductNaam = updatedProduct.ProductNaam;
            }

            if (updatedProduct.ProductPrijs > 0)
            {
                ProductPrijs = updatedProduct.ProductPrijs;
            }

            if (updatedProduct.ProductKorting >= 0)
            {
                ProductKorting = updatedProduct.ProductKorting;
            }
        }
    }
}
