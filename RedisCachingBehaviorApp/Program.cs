using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
class Program
{
    static async Task Main(string[] args)
    {
        var services = new ServiceCollection();
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = "localhost:6379";
            options.InstanceName = "CacheExample";
        });
        var provider = services.BuildServiceProvider();
        var cache = provider.GetRequiredService<IDistributedCache>();
        Console.WriteLine("Redis cache setup complete.");

        var cacheEntryOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10), // Expires in 10 minutes
            SlidingExpiration = TimeSpan.FromSeconds(2) // Resets if accessed within 2 minutes
        };
        // Store a value in the cache
        await cache.SetStringAsync("taskKey", "Sample Task", cacheEntryOptions);
        Console.WriteLine("Cache entry set with absolute and sliding expiration.");
        // Retrieve and display cached value
        var cachedValue = await cache.GetStringAsync("taskKey");
        Console.WriteLine(cachedValue != null ? $"Cache hit: {cachedValue}" : "Cache miss: Value expired.");

        async Task InvalidateCache(IDistributedCache cache, string key)
        {
            await cache.RemoveAsync(key);
            Console.WriteLine($"Cache entry '{key}' has been invalidated.");
        }
        // Trigger cache invalidation
        Console.WriteLine("Press any key to invalidate cache...");
        Console.ReadKey();
        await InvalidateCache(cache, "taskKey");

        async Task<string> GetCachedData(IDistributedCache cache, string key)
        {
            var cachedValue = await cache.GetStringAsync(key);

            if (cachedValue != null)
            {
                Console.WriteLine($"Cache hit: {cachedValue}");
                return cachedValue;
            }

            Console.WriteLine("Cache miss: Fetching new data...");
            var newValue = "Fetched Task Data";
            await cache.SetStringAsync(key, newValue, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                SlidingExpiration = TimeSpan.FromMinutes(2)
            });
            Console.WriteLine("New data cached.");
            return newValue;
        }
        // Simulate repeated cache access
        for (int i = 0; i < 5; i++)
        {
            await GetCachedData(cache, "taskKey");
            await Task.Delay(TimeSpan.FromSeconds(10)); // Simulate waiting time
        }
    }
}