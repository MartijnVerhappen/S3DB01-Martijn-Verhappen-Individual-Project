using Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class WinkelmandEntity
    {
        public int Id { get; set; }
        public int KlantId { get; set; }
        public Klant Klant { get; set; }
        public DateTime AanmaakDatum { get; set; }
        public DateTime LaatsteVeranderingsDatum { get; set; }
        public ICollection<WinkelmandProductEntity> WinkelmandProducts { get; set; }
    }
}
