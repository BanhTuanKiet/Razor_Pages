
namespace WebApplication1.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }

        // Navigation
        public User? User { get; set; }
        public List<OrderDetail>? OrderDetails { get; set; }
    }
}
