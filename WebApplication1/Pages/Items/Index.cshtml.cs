using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Pages.Items
{
    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;
        public decimal Price { get; set; }
        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public void OnGet() { }

        // GET: /Items?handler=All
        public IActionResult OnGetAll()
        {
            var items = _context.Items
                .Select(i => new {
                    id    = i.Id,
                    name  = i.Name,
                    price = i.Price,
                    stock = i.Stock
                })
                .ToList();

            return new JsonResult(items);
        }

        // POST: /Items?handler=Create
        public async Task<IActionResult> OnPostCreateAsync([FromBody] ItemDto dto)
        {
            try
            {
                var item = new Item
                {
                    Name  = dto.Name,
                    Price = dto.Price,
                    Stock = dto.Stock
                };

                _context.Items.Add(item);
                await _context.SaveChangesAsync();

                return new JsonResult(new { success = true, id = item.Id });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, error = ex.Message })
                {
                    StatusCode = 500
                };
            }
        }

        // POST: /Items?handler=Update&id=1
        public async Task<IActionResult> OnPostUpdateAsync(int id, [FromBody] ItemDto dto)
        {
            try
            {
                var item = await _context.Items.FindAsync(id);
                if (item == null)
                    return new JsonResult(new { success = false, error = "Không tìm thấy item" });

                item.Name  = dto.Name;
                item.Price = dto.Price;
                item.Stock = dto.Stock;

                await _context.SaveChangesAsync();

                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, error = ex.Message })
                {
                    StatusCode = 500
                };
            }
        }

        // POST: /Items?handler=Delete&id=1
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                var item = await _context.Items.FindAsync(id);
                if (item == null)
                    return new JsonResult(new { success = false, error = "Không tìm thấy item" });

                // Kiểm tra item có đang được dùng trong OrderDetails không
                bool isUsed = _context.OrderDetails.Any(od => od.ItemId == id);
                if (isUsed)
                    return new JsonResult(new
                    {
                        success = false,
                        error = "Không thể xóa, item đang có trong đơn hàng"
                    });

                _context.Items.Remove(item);
                await _context.SaveChangesAsync();

                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, error = ex.Message })
                {
                    StatusCode = 500
                };
            }
        }
    }

    // DTO nhận data từ body
    public class ItemDto
    {
        public string Name  { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock    { get; set; }
    }
}