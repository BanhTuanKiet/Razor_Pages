
namespace WebApplication1.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public decimal? Price { get; set; }

        // Navigation
        public Order? Order { get; set; }
        public Item? Item { get; set; }
    }
}