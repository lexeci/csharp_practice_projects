using LogiTrack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using Microsoft.Extensions.Caching.Memory;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Require authentication for all actions in this controller
public class OrderController : ControllerBase
{
    private readonly LogiTrackContext _context;
    private readonly IMemoryCache _cache;
    private const string OrdersCacheKey = "ordersList";

    // Constructor injecting database context and memory cache service
    public OrderController(LogiTrackContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    // GET: /api/orders
    // Retrieves all orders including their items, with caching for performance
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
    {
        var stopwatch = Stopwatch.StartNew();

        // Try to get orders list from cache
        if (!_cache.TryGetValue(OrdersCacheKey, out List<Order>? orders))
        {
            // If not found in cache, load from database with related items
            orders = await _context.Orders
                .Include(o => o.Items)  // Include child items
                .AsNoTracking()         // Disable change tracking for performance
                .ToListAsync();

            // Set cache expiration time to 5 minutes
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

            // Store the loaded orders in cache
            _cache.Set(OrdersCacheKey, orders, cacheEntryOptions);
        }

        stopwatch.Stop();

        // Log how long it took to load orders (for debugging/performance monitoring)
        Console.WriteLine($"Orders loaded in {stopwatch.ElapsedMilliseconds} ms");

        // Return the orders list along with the elapsed time
        return Ok(new
        {
            TimeElapsedMs = stopwatch.ElapsedMilliseconds,
            Data = orders,
        });
    }

    // GET: /api/orders/{id}
    // Retrieves a single order by ID including its items
    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrder(int id)
    {
        // Find order by id with related items
        var order = await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.OrderId == id);

        if (order == null)
            return NotFound($"Order with ID {id} not found.");

        return Ok(order);
    }

    // POST: /api/orders
    // Creates a new order; requires Manager or Admin roles
    [HttpPost]
    [Authorize(Roles = "Manager,Admin")]
    public async Task<IActionResult> CreateOrder([FromBody] Order order)
    {
        if (order == null)
            return BadRequest("Order cannot be null.");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (string.IsNullOrWhiteSpace(order.CustomerName))
            return BadRequest("Customer name is required.");

        // Reset OrderId to 0 to let the database generate a new ID
        order.OrderId = 0;

        // Validate and reset IDs for order items (child entities)
        if (order.Items != null)
        {
            foreach (var item in order.Items)
            {
                if (string.IsNullOrWhiteSpace(item.Name))
                    return BadRequest("Item name is required.");
                if (item.Quantity < 1)
                    return BadRequest("Item quantity must be greater than 0.");

                item.Order = order;  // Set navigation property to the parent order
                item.ItemId = 0;     // Reset item ID for new record insertion
            }
        }

        try
        {
            // Add new order to context and save changes
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            // Return 500 Internal Server Error with database error details
            return StatusCode(500, $"Database update failed: {ex.InnerException?.Message ?? ex.Message}");
        }

        // Return 201 Created with location header pointing to the new resource
        return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, order);
    }

    // DELETE: /api/orders/{id}
    // Deletes an order and its items; requires Manager or Admin roles
    [HttpDelete("{id}")]
    [Authorize(Roles = "Manager,Admin")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        // Find order including its items
        var order = await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.OrderId == id);

        if (order == null)
            return NotFound($"Order with ID {id} not found.");

        try
        {
            // Remove all child items first to avoid foreign key conflicts
            _context.InventoryItems.RemoveRange(order.Items);
            // Remove the order itself
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            // Remove cached orders list so next GET will reload fresh data
            _cache.Remove(OrdersCacheKey);
        }
        catch (DbUpdateException ex)
        {
            // Return 500 Internal Server Error if deletion fails
            return StatusCode(500, $"Error deleting order: {ex.InnerException?.Message ?? ex.Message}");
        }

        // Return 204 No Content to indicate successful deletion
        return NoContent();
    }
}
