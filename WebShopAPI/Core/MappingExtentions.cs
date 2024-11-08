using Core.DTO_s;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public static class MappingExtensions
    {
        public static ProductModel ToProductModel(this ProductDTO dto, ProductModel existingModel = null)
        {
            if (existingModel == null)
            {
                return new ProductModel(dto.Id, dto.ProductType, dto.ProductNaam, dto.ProductPrijs, dto.ProductKorting);
            }
            else
            {
                existingModel.Update(dto.ProductType, dto.ProductNaam, dto.ProductPrijs, dto.ProductKorting);
                return existingModel;
            }
        }
    }
}
