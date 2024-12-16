using DAL.Entities;
using Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Mapping
{
    public class KlantMapping
    {
        public static Klant MapTo(KlantEntity klantEntity)
        {
            Klant klant = new Klant
            {
                Id = klantEntity.Id,
                Gebruikersnaam = klantEntity.Gebruikersnaam,
                WachtwoordHash = klantEntity.WachtwoordHash,
                MFAStatus = klantEntity.MFAStatus,
                MFAVorm = klantEntity.MFAVorm,
            };    
            return klant;
        }
        public static KlantEntity MapTo(Klant klant)
        {
            KlantEntity entity = new KlantEntity
            {
                Id = klant.Id,
                Gebruikersnaam = klant.Gebruikersnaam,
                WachtwoordHash = klant.WachtwoordHash,
                MFAStatus = klant.MFAStatus,
                MFAVorm = klant.MFAVorm
            };  
            return entity;
        }
    }
}
