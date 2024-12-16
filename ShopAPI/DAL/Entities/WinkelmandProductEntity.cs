using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class WinkelmandProductEntity
    {
        public int Id { get; set; }
        public int WinkelmandId { get; set; }
        public WinkelmandEntity Winkelmand { get; set; }
        public int ProductId { get; set; }
        public ProductEntity Product { get; set; }
        public int aantal {  get; set; }
    }
}
