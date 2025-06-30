using LogiTrack;
using LogiTrack.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

// DbContext class for the LogiTrack application,
// extends IdentityDbContext to include Identity user management
public class LogiTrackContext : IdentityDbContext<ApplicationUser>
{
    // Constructor that accepts DbContextOptions and passes them to base class
    public LogiTrackContext(DbContextOptions<LogiTrackContext> options)
        : base(options)
    {
    }

    // DbSet representing Orders table in the database
    public DbSet<Order> Orders { get; set; }

    // DbSet representing InventoryItems table in the database
    public DbSet<InventoryItem> InventoryItems { get; set; }

    // Configure entity relationships and constraints
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Call base implementation to ensure Identity configurations are applied
        base.OnModelCreating(modelBuilder);

        // Configure one-to-many relationship between Order and InventoryItem:
        // Each InventoryItem belongs to one Order,
        // Each Order can have many InventoryItems.
        // Set foreign key OrderId on InventoryItem.
        // Enable cascade delete: deleting an Order deletes related InventoryItems.
        modelBuilder.Entity<InventoryItem>()
            .HasOne(i => i.Order)
            .WithMany(o => o.Items)
            .HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
