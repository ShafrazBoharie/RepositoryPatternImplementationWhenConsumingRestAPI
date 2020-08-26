using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Amido.Client.Models;

namespace Amido.Client.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProducts();
        Task<Product> GetProduct(int id);
        Task<Product> AddProduct(Product product);

        Task<Product> UpdateProduct(Product product);

        Task DeleteProduct(int id);
    }
}
