using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Logic.Models
{
    [Table("Product")]
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductType { get; set; }

        [Required]
        [StringLength(100)]
        public string ProductNaam { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double ProductPrijs { get; set; }

        [Required]
        [Range(0, 100)]
        public int ProductKorting { get; set; }

        public Product()
        {
        }

        public Product(int id, string productType, string productNaam, double productPrijs, int productKorting)
        {
            Id = id;
            ProductType = productType;
            ProductNaam = productNaam;
            ProductPrijs = productPrijs;
            ProductKorting = productKorting;
        }
    }
}
