using Microsoft.Extensions.Caching.Memory;

IMemoryCache cache = new MemoryCache(new MemoryCacheOptions { SizeLimit = 1024 });

cache.Set("ProductList", new List<string> { "Product1", "Product2" },
    new MemoryCacheEntryOptions
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
        Size = 1
    });


if (!cache.TryGetValue("ProductList", out List<string>? products))
{
    Console.WriteLine("Cache miss. Fetching data...");
    products = new List<string> { "Product1", "Product2", "Product3" }; // Simulate fetching data

    cache.Set("ProductList", products, new MemoryCacheEntryOptions
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
        Size = 1
    });
    Console.WriteLine("Data added to cache.");
}
else
{
    Console.WriteLine("Cache hit. Data retried from cache");
}

Console.WriteLine($"Cached Data: {string.Join(", ", products)}");

Console.WriteLine("Removing 'ProductList' from cache...");

cache.Remove("ProductList");
if (!cache.TryGetValue("ProductList", out _))
{
    Console.WriteLine("Cache entry 'ProductList' successfully removed.");
}
else
{
    Console.WriteLine("Cache entry 'ProductList' still exists.");

}