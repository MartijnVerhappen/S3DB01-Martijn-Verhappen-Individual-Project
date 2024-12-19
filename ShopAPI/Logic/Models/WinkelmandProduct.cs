using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Models
{
    public class WinkelmandProduct
    {
        public int Id { get; set; }
        public int WinkelmandId { get; set; }
        public Winkelmand Winkelmand { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int aantal { get; set; }
    }
}
