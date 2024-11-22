using Core.IRepositories;
using Core.IServices;
using Core.Models;
using Core.DTO_s;
using Core;
using Microsoft.AspNetCore.Mvc;

namespace Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        private void MapDtoToModel(ProductDTO dto, ProductModel model)
        {
            model.Update(dto.ProductType, dto.ProductNaam, dto.ProductPrijs, dto.ProductKorting);
        }

        public async Task<List<ProductModel>> GetAllProducts()
        {
            return await _productRepository.GetAllProducts();
        }

        public async Task<ProductModel> GetProductById(int id)
        {
            return await _productRepository.GetProductById(id);
        }

        public async Task<ProductModel> UpdateProduct(int id, ProductDTO updatedProductDto)
        {
            var existingProduct = await _productRepository.GetProductById(updatedProductDto.Id);
            if (existingProduct == null)
                throw new Exception("Product not found");

            existingProduct.ApplyChanges(updatedProductDto);

            await _productRepository.UpdateProduct(existingProduct);

            return existingProduct;
        }
    }
}
