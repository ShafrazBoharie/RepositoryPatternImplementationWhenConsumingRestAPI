using System;
using Amido.Client.Models;
using Amido.Client.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Amido.Client.Services
{
    public class ProductService:IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        
        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _productRepository.GetProducts();
        }

        public async Task<Product> GetProduct(int id)
        {
            if (id < 0) throw new Exception(("Invalid Id"));
            return await _productRepository.GetProduct(id);
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            return await _productRepository.UpdateProduct(product);
        }

        public async Task DeleteProduct(int id)
        {
            await _productRepository.DeleteProduct(id);
        }
    }
}
