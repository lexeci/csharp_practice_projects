using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LogiTrack
{
    public class Order
    {
        // Primary key for the Order entity
        [Key]
        public int OrderId { get; set; }

        // Customer's name, required field
        [Required]
        public required string CustomerName { get; set; }

        // Date when the order was placed, defaults to current date/time
        public DateTime DatePlaced { get; set; } = DateTime.Now;

        // Collection of related InventoryItem objects in this order
        public List<InventoryItem> Items { get; set; } = new();

        // Adds an InventoryItem to the order and sets its foreign key OrderId
        public void AddItem(InventoryItem item)
        {
            item.OrderId = this.OrderId;
            Items.Add(item);
        }

        // Removes all items from the order matching the specified ItemId
        public void RemoveItem(int itemId)
        {
            Items.RemoveAll(i => i.ItemId == itemId);
        }

        // Returns a short summary string describing the order
        public string GetOrderSummary()
        {
            return $"Order #{OrderId} for {CustomerName} | Items: {Items.Count} | Placed: {DatePlaced.ToShortDateString()}";
        }

        // Returns a detailed multi-line string summarizing the order and all its items
        public string GetOrderSummaryDetailed()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Order #{OrderId} for {CustomerName} | Placed: {DatePlaced.ToShortDateString()}");
            foreach (var item in Items)
            {
                sb.AppendLine($"- {item.Name} x{item.Quantity} @ {item.Location}");
            }
            return sb.ToString();
        }
    }
}
