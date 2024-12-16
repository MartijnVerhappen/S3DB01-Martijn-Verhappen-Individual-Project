using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class ProductEntity
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

        public ProductEntity()
        {
        }

        public ProductEntity(int id, string productType, string productNaam, double productPrijs, int productKorting)
        {
            Id = id;
            ProductType = productType;
            ProductNaam = productNaam;
            ProductPrijs = productPrijs;
            ProductKorting = productKorting;
        }
    }
}
