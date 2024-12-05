using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Models
{
    public class Klant
    {
        public int Id { get; set; }
        public string Gebruikersnaam { get; set; }
        public string WachtwoordHash { get; set; }
        public bool MFAStatus { get; set; }
        public string MFAVorm {  get; set; }
        public Winkelmand Winkelmand { get; set; }
    }
}
