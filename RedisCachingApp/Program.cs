using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
class Program
{
    static async Task Main(string[] args)
    {
        var services = new ServiceCollection();
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = "localhost:6379";
            options.InstanceName = "SampleInstance";
        });
        var provider = services.BuildServiceProvider();
        var cache = provider.GetService<IDistributedCache>();
        string cacheKey = "ProductList";
        string cachedData = await cache.GetStringAsync(cacheKey);
        if (cachedData != null)
        {
            Console.WriteLine("Cache Hit: " + cachedData);
        }
        else
        {
            Console.WriteLine("Cache Miss: Generating new data...");
            string productData = "Product1, Product2, Product3";
            var options = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
            await cache.SetStringAsync(cacheKey, productData, options);
            Console.WriteLine("Data Stored in Cache: " + productData);
        }
    }
}