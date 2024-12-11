using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Models
{
    public class Winkelmand
    {
        public int Id { get; set; }
        public int KlantId { get; set; }
        public Klant Klant { get; set; }
        public DateTime AanmaakDatum { get; set; }
        public DateTime LaatsteVeranderingsDatum { get; set; }
        public ICollection<WinkelmandProduct> WinkelmandProduct { get; set; }
    }
}
