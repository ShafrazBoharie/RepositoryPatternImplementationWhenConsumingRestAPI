using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amido.Client.Models;
using Amido.Client.Repository;
using Amido.Client.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace Amido.Client.Tests
{
    public class ProductServiceTests
    {
        private Mock<IProductRepository> _productRepository;
        private ProductService productService;

        public ProductServiceTests()
        {
            _productRepository = new Mock<IProductRepository>();
            ConfigureProductRepoMock();
            productService = new ProductService(_productRepository.Object);
        }

        [Fact]
        public async Task GivenARequestToRetrieveAllProducts()
        {
            var products = await productService.GetAllProducts();
            products.Count().Should().Be(3);
        }

        [Fact]
        public async Task GivenARequestToRetrieveAProduct_WithValidId()
        {
            var id = 2;
            var product = await productService.GetProduct(id);
            product.Should().BeEquivalentTo(
                new Product()
                {
                    Id = 1,
                    Name = "AAA",
                    Category = "Cloud",
                    Price = 999
                });
        }

        [Fact]
        public async Task GivenARequestToRetrieveAProduct_WithIbnvalidIdShouldThrowException()
        {
            var id = -1;
            var exception = Assert.ThrowsAsync<Exception>(() => productService.GetProduct(id));
            exception.Result.Message.Should().Be("Invalid Id");
        }

        [Fact]
        public async Task GivenARequestToUpdateAProduct()
        {
            var product = new Product
            {
                Id = 1,
                Name = "AAA",
                Category = "Cloud",
                Price = 999
            };
            var updatedProduct = await productService.UpdateProduct(product);
            updatedProduct.Should().BeEquivalentTo(
                new Product()
                {
                    Id = 1,
                    Name = "Updated",
                    Category = "Cloud",
                    Price = 999
                });
        }

        [Fact]
        public async Task GivenARequestToDeleteAProduct()
        {
            var id = 2;
            await productService.DeleteProduct(id);
            _productRepository.Verify(x => x.DeleteProduct(It.IsAny<int>()), Times.Once);
        }


        private void ConfigureProductRepoMock()
        {
            _productRepository.Setup(x => x.GetProducts()).ReturnsAsync(
                new List<Product>
                {
                    new Product()
                    {
                        Id = 1, Name = "AAA", Category = "Cloud", Price = 999
                    },
                    new Product()
                    {
                        Id = 2, Name = "BBB", Category = "Server", Price = 2344
                    },
                    new Product()
                    {
                    Id = 3, Name = "CCC", Category = "ClientSupport", Price = 44
                }
                });

            _productRepository.Setup(x => x.GetProduct(It.IsAny<int>())).ReturnsAsync(
                new Product()
                {
                    Id = 1,
                    Name = "AAA",
                    Category = "Cloud",
                    Price = 999
                });

            _productRepository.Setup(x => x.UpdateProduct(It.IsAny<Product>())).ReturnsAsync(
                new Product()
                {
                    Id = 1,
                    Name = "Updated",
                    Category = "Cloud",
                    Price = 999
                });

            _productRepository.Setup(x => x.DeleteProduct(It.IsAny<int>()));

        }
    }
}
