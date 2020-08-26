using System;
using System.Net.Http;
using Amido.Client.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Amido.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = CreateServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var app = scope.ServiceProvider.GetRequiredService<Application>();

            app.Execute();

            Console.ReadKey();
        }

        private static IServiceProvider CreateServiceProvider()
        {
            var configuration = CreateConfiguration();
            var services = new ServiceCollection();

            services.AddOptions();

            services.AddLogging(configure => configure.AddConsole());
            services.AddScoped<IProductRepository>(_ =>
            {
                var serverUrl = configuration.GetValue<string>("ApiClient:Url");
               
                var client = new HttpClient()
                {
                    BaseAddress = new Uri(serverUrl)
                };
                var productRepository = new ProductRepository(client);
                return productRepository;
            });
            services.AddScoped<Application>();


            return services.BuildServiceProvider(true);
        }

        private static IConfiguration CreateConfiguration()
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json");

            return builder.Build();
        }
    }
}
