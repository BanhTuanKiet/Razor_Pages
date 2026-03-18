using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1.Pages.Orders
{
    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public void OnGet() { }

        public IActionResult OnGetOrdersByUser(int userId)
        {
            try
            {
                // Dùng DB thật:
                // var orders = _context.Orders
                //     .Where(o => o.UserId == userId)
                //     .Select(o => new { id = o.Id, orderDate = o.OrderDate })
                //     .ToList();

                // Dữ liệu giả để test:
                var orders = new List<object>
                {
                    new { id = 1, orderDate = "2024-01-10" },
                    new { id = 2, orderDate = "2024-02-15" },
                    new { id = 3, orderDate = "2024-03-20" }
                };

                return new JsonResult(orders);
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message })
                {
                    StatusCode = 500
                };
            }
        }
    }
}