using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.DTOs;
using WebApplication1.Services;
using WebApplication1.Services.Order;

namespace WebApplication1.Pages.Orders
{
    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        private readonly IOrderServcie _orderService;

        public IndexModel(IOrderServcie orderService)
        {
            _orderService = orderService;
        }

        public IActionResult OnGetAll()
        {
            var items = _orderService.GetAll();

            return new JsonResult(items);
        }

        public async Task<JsonResult> OnPostOrders([FromBody] FilterDto.AgGridRequest request)
        {
            var userId = Request.Query.ContainsKey("userId")
                ? int.Parse(Request.Query["userId"]!)
                : (int?)null;

            var result = await _orderService.GetOrders(request, userId);
            return new JsonResult(result);
        }
    }
}