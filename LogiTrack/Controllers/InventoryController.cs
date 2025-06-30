using LogiTrack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Protects entire controller, requires authorization to access any endpoint
public class InventoryController : ControllerBase
{
    private readonly LogiTrackContext _context;
    private readonly IMemoryCache _cache;
    private const string CacheKey = "inventoryList";

    public InventoryController(LogiTrackContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    // GET /api/inventory
    // Retrieves all inventory items, caching results for 5 minutes to improve performance
    [HttpGet]
    public async Task<ActionResult<IEnumerable<InventoryItem>>> GetInventoryItems()
    {
        var stopwatch = Stopwatch.StartNew(); // Start timing the operation

        // Attempt to get inventory from cache
        if (!_cache.TryGetValue(CacheKey, out List<InventoryItem>? inventory))
        {
            // If not cached, load from database
            inventory = await _context.InventoryItems
                .AsNoTracking() // Improves performance by disabling change tracking for read-only query
                .ToListAsync();

            // Set cache expiration to 5 minutes
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

            // Store the inventory list in cache
            _cache.Set(CacheKey, inventory, cacheEntryOptions);
        }

        stopwatch.Stop(); // Stop timing

        // Log the time taken to load inventory (consider replacing with proper logging)
        Console.WriteLine($"Inventory loaded in {stopwatch.ElapsedMilliseconds} ms");

        // Return the elapsed time and data in the response
        return Ok(new
        {
            TimeElapsedMs = stopwatch.ElapsedMilliseconds,
            Data = inventory
        });
    }

    // POST /api/inventory
    // Adds a new inventory item; restricted to users with Manager or Admin roles
    [HttpPost]
    [Authorize(Roles = "Manager,Admin")]
    public async Task<ActionResult<InventoryItem>> AddInventoryItem([FromBody] InventoryItem item)
    {
        if (item == null)
            return BadRequest("Inventory item cannot be null.");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Validate required fields
        if (string.IsNullOrWhiteSpace(item.Name))
            return BadRequest("Item name is required.");

        if (item.Quantity < 1)
            return BadRequest("Quantity must be greater than 0.");

        if (string.IsNullOrWhiteSpace(item.Location))
            return BadRequest("Location is required.");

        // Verify the associated OrderId exists in Orders table
        if (!await _context.Orders.AnyAsync(o => o.OrderId == item.OrderId))
            return BadRequest($"Order with ID {item.OrderId} does not exist.");

        try
        {
            // Add the new inventory item to the database
            _context.InventoryItems.Add(item);
            await _context.SaveChangesAsync();

            // Remove cached inventory list to keep cache consistent
            _cache.Remove(CacheKey);
        }
        catch (DbUpdateException ex)
        {
            // Return 500 status code with detailed error message on failure
            return StatusCode(500, $"Database update failed: {ex.InnerException?.Message ?? ex.Message}");
        }

        // Return 201 Created response with the created item data
        return CreatedAtAction(nameof(GetInventoryItems), new { id = item.ItemId }, item);
    }

    // DELETE /api/inventory/{id}
    // Deletes an inventory item by ID; restricted to users with Manager or Admin roles
    [HttpDelete("{id}")]
    [Authorize(Roles = "Manager,Admin")]
    public async Task<IActionResult> DeleteInventoryItem(int id)
    {
        // Find the inventory item by ID
        var item = await _context.InventoryItems.FindAsync(id);
        if (item == null)
            return NotFound($"Inventory item with ID {id} not found.");

        try
        {
            // Remove the item from database
            _context.InventoryItems.Remove(item);
            await _context.SaveChangesAsync();

            // Clear the cached inventory list
            _cache.Remove(CacheKey);
        }
        catch (DbUpdateException ex)
        {
            // Return 500 status code with detailed error message on failure
            return StatusCode(500, $"Error deleting item: {ex.InnerException?.Message ?? ex.Message}");
        }

        // Return 204 No Content to indicate successful deletion
        return NoContent();
    }
}
