using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class KlantEntity
    {
        public int Id { get; set; }
        public string Gebruikersnaam { get; set; }
        public string WachtwoordHash { get; set; }
        public bool MFAStatus { get; set; }
        public string MFAVorm { get; set; }
        public WinkelmandEntity Winkelmand { get; set; }
    }
}
