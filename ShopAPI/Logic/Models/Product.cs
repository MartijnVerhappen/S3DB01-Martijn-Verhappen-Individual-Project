using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Models
{
    [Table("Product")]
    public class Product
    {
        public int Id { get; set; }
        public string ProductType { get; set; }
        public string ProductNaam { get; set; }
        public double ProductPrijs { get; set; }
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
