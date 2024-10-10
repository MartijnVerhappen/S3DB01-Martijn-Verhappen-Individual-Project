namespace Web_Shop_API.Models
{
    public class ProductModel
    {
        public int Id { get; private set; }

        public string ProductType { get; private set; }

        public string ProductNaam {  get; private set; }

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
    }
}
