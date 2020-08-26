using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Amido.Client.Repository;

namespace Amido.Client
{
    public class Application
    {
        private readonly IProductRepository _productRepository;

        public Application(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public void Execute()
        {
            GetAllProducts().Wait();
        }

        private async Task GetAllProducts()
        {
            var products = await _productRepository.GetProducts();

            foreach (var p in products)
            {
                Console.WriteLine($"{p.Id} - {p.Name}");
            }
        }
    }
}
