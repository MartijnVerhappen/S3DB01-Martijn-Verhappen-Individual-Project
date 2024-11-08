namespace Core.DTO_s
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string ProductType { get; set; }
        public string ProductNaam { get; set; }
        public double ProductPrijs { get; set; }
        public int ProductKorting { get; set; }

        public ProductDTO(int id, string productType, string productnaam, double productPrijs, int productKorting)
        {
            Id = id;
            ProductType = productType;
            ProductNaam = productnaam;
            ProductPrijs = productPrijs;
            ProductKorting = productKorting;
        }
    }
}
