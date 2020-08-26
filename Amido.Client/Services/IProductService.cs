using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Amido.Client.Models;

namespace Amido.Client.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> GetProduct(int id);
        Task<Product> UpdateProduct(Product product);
        Task DeleteProduct(int id);
    }
}
