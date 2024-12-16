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
    public class WinkelmandMapping
    {
        public static Winkelmand MapTo(WinkelmandEntity winkelmandEntity)
        {
            Winkelmand winkelmand = new Winkelmand
            {
                Id = winkelmandEntity.Klant.Winkelmand.Id,
                KlantId = winkelmandEntity.Id,
                AanmaakDatum = winkelmandEntity.Klant.Winkelmand.AanmaakDatum,
                LaatsteVeranderingsDatum = winkelmandEntity.Klant.Winkelmand.LaatsteVeranderingsDatum,
                Products = ProductMapping.MapTo(winkelmandEntity.Products)
            };
            return winkelmand;
        }

        public static WinkelmandEntity MapTo(Winkelmand winkelmand)
        {
            WinkelmandEntity winkelmandEntity = new WinkelmandEntity
            {
                Id = winkelmand.Id,
                KlantId = winkelmand.KlantId,
                AanmaakDatum = winkelmand.AanmaakDatum,
                LaatsteVeranderingsDatum = winkelmand.LaatsteVeranderingsDatum,
                Products = ProductMapping.MapTo(winkelmand.Products)
            };
            return winkelmandEntity;
        }
    }
}
