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
        public static Winkelmand MapTo(WinkelmandEntity winkelmandEntity, Klant klant)
        {
            var winkelmandProducts = winkelmandEntity.WinkelmandProducts.Select(wp => new WinkelmandProduct
            {
                Id = wp.Id,
                ProductId = wp.ProductId,
                aantal = wp.aantal,
                Product = new Product
                {
                    Id = wp.Product.Id,
                    ProductType = wp.Product.ProductType,
                    ProductNaam = wp.Product.ProductNaam,
                    ProductPrijs = wp.Product.ProductPrijs,
                    ProductKorting = wp.Product.ProductKorting
                }
            }).ToList();

            var winkelmand = new Winkelmand
            {
                Id = winkelmandEntity.Id,
                KlantId = klant.Id,
                AanmaakDatum = winkelmandEntity.AanmaakDatum,
                LaatsteVeranderingsDatum = winkelmandEntity.LaatsteVeranderingsDatum,
                WinkelmandProducts = winkelmandProducts
            };

            return winkelmand;
        }

        public static WinkelmandEntity MapTo(Winkelmand winkelmand)
        {
            var winkelmandProducts = winkelmand.WinkelmandProducts.Select(wp => new WinkelmandProductEntity
            {
                Id = wp.Id,
                WinkelmandId = winkelmand.Id,
                ProductId = wp.ProductId,
                Product = ProductMapping.MapTo(wp.Product),
                aantal = wp.aantal
            }).ToList();

            var winkelmandEntity = new WinkelmandEntity
            {
                Id = winkelmand.Id,
                KlantId = winkelmand.KlantId,
                Klant = new Klant { Id = winkelmand.KlantId },
                AanmaakDatum = winkelmand.AanmaakDatum,
                LaatsteVeranderingsDatum = winkelmand.LaatsteVeranderingsDatum,
                WinkelmandProducts = winkelmandProducts
            };

            return winkelmandEntity;
        }
    }
}
