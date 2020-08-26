using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Amido.Client.Models;
using Newtonsoft.Json;

namespace Amido.Client.Repository
{
    public class ProductRepository:IProductRepository
    {
        private readonly HttpClient _client;
        private const string path = "/api/Products";

        public ProductRepository(HttpClient client)
        {
            _client = client;
        }
        public async Task<IEnumerable<Product>> GetProducts()
        {
            var response = await _client.GetAsync(path);
            var content = await response.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(content);
            return products;
        }

        public async Task<Product> GetProduct(int id)
        {
            var response = await _client.GetAsync($"{path}/{id}");
            var content = await response.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<Product>(content);
            return product;
        }

        public async Task<Product> AddProduct(Product product)
        {
            var response = await _client.PostAsync($"{path}", new StringContent(JsonConvert.SerializeObject(product)));
            var content = await response.Content.ReadAsStringAsync();
            var newproduct = JsonConvert.DeserializeObject<Product>(content);

            return newproduct;
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            var content = new StringContent(JsonConvert.SerializeObject(product));
            var response = await _client.PutAsync($"{path}", content);
            var updatedProduct = JsonConvert.DeserializeObject<Product>(await response.Content.ReadAsStringAsync());

            return updatedProduct;
        }

        public async Task DeleteProduct(int id)
        {
            var response = await _client.DeleteAsync($"{path}/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
