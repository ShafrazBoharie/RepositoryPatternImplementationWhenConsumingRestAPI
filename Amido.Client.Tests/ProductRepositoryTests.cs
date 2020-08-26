using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amido.Client.Models;
using Amido.Client.Repository;
using FluentAssertions;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Xunit;

namespace Amido.Client.Tests
{
    public class ProductRepositoryTests
    {
        

        [Fact]
        public async Task GivenARequestToRetrieveAllProducts_ShouldRetrieveAllProducts()
        {
            var restUrl = "http://testserver.com";
            var messageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            messageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(
                        (new List<Product>() { new Product { Id = 1, Name = "Test", Price = (decimal)12.0, Category = "AAAA" } }).AsEnumerable())
                    , Encoding.UTF32, "application/json")
                }).Verifiable();

            var httpClient = new HttpClient(messageHandler.Object)
            {
                BaseAddress = new Uri(restUrl)
            };

            var productRepository=new ProductRepository(httpClient);

            var products = await productRepository.GetProducts();
            products.Should().NotBeNull();
            products.FirstOrDefault().Id.Should().Be(1);
        }

        [Fact]
        public async Task GivenARequestToRetrieveAllProductsWithInvalidRequest_ShouldThrowException()
        {
            // Act
            var restApiUrl = "http://test.com";
            var messageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            messageHandler.Protected() //Setup the PROTECTED method to mock 
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .Throws<HttpRequestException>()
                .Verifiable();

            var httpClient = new HttpClient(messageHandler.Object)
            {
                BaseAddress = new Uri(restApiUrl)
            };

            var productRepository = new ProductRepository(httpClient);

            var exception = await Assert.ThrowsAsync<HttpRequestException>(() => productRepository.GetProduct(1));

            exception.Should().BeOfType<HttpRequestException>();

        }

        [Fact]
        public async Task GivenANewProductShouldAddToTheRepository()
        {
            var item = new Product { Name = "Test", Price = (decimal)12.0, Category = "AAAA" };

            // Act
            var restApiUrl = "http://test.com";
            var messageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            messageHandler.Protected() //Setup the PROTECTED method to mock 
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.Created,
                    Content = new StringContent(
                            JsonConvert.SerializeObject(item), Encoding.UTF32, "application/json")
                }
                ).Verifiable();

            var httpClient = new HttpClient(messageHandler.Object)
            {
                BaseAddress = new Uri(restApiUrl)
            };

            var productRepository = new ProductRepository(httpClient);

            var products = await productRepository.AddProduct(item);
            products.Should().NotBeNull();
            products.Name.Should().Be("Test");

        }

        [Fact]
        public async Task GivenExistingProductShouldUpdateItsContent()
        {
            var item = new Product { Name = "TestUpdated", Price = (decimal)12.0, Category = "AAAA" };

            // Act
            var restApiUrl = "http://test.com";
            var messageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            messageHandler.Protected() //Setup the PROTECTED method to mock 
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.Created,
                    Content = new StringContent(
                            JsonConvert.SerializeObject(item), Encoding.UTF32, "application/json")
                }
                ).Verifiable();

            var httpClient = new HttpClient(messageHandler.Object)
            {
                BaseAddress = new Uri(restApiUrl)
            };

            var productRepository = new ProductRepository(httpClient);

            var products = await productRepository.UpdateProduct(item);
            products.Should().NotBeNull();
            products.Name.Should().Be("TestUpdated");

        }
    }
}
