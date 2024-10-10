namespace Web_Shop_API.Entities
{
    public class ProductEntity
    {
        public int Id { get; private set; }

        public string ProductType { get; private set; }

        public string ProductNaam {  get; private set; }

        public double ProductPrijs { get; private set; }

        public int ProductKorting { get; private set; }


        public ProductEntity(int id, string productType, string productnaam, double productPrijs, int productKorting) 
        { 
            Id = id;
            ProductType = productType;
            ProductNaam = productnaam;
            ProductPrijs = productPrijs;
            ProductKorting = productKorting;
        }
    }
}
