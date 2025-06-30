using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogiTrack
{
    public class InventoryItem
    {
        // Primary key for InventoryItem entity
        [Key]
        public int ItemId { get; set; }

        // Name of the inventory item, required field
        [Required]
        public required string Name { get; set; }

        // Quantity of this item in stock
        public int Quantity { get; set; }

        // Location where the item is stored, required field
        public required string Location { get; set; }

        // Foreign key to the related Order entity (nullable)
        [ForeignKey("OrderId")]
        public int? OrderId { get; set; }

        // Navigation property to the associated Order
        public Order? Order { get; set; }

        // Method to print basic information about the item to the console
        public void DisplayInfo()
        {
            Console.WriteLine($"Item: {Name} | Quantity: {Quantity} | Location: {Location}");
        }
    }
}
