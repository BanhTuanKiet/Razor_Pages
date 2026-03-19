using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DTOs;
using WebApplication1.Models;
using WebApplication1.Services.Item;

namespace WebApplication1.Pages.Items
{
    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        private readonly IItemService _itemService;

        public IndexModel(IItemService itemService)
        {
            _itemService = itemService;
        }

        public void OnGet() { }

        public async Task<JsonResult> OnPostItems([FromBody] FilterDto.AgGridRequest request)
        {
            var result = await _itemService.GetItems(request);
            return new JsonResult(result);
        }

        // GET: /Items?handler=All
        public IActionResult OnGetAll()
        {
            var items = _itemService.GetAll();

            return new JsonResult(items);
        }

        // POST: /Items?handler=Delete&id=1
        public async Task<IActionResult> OnPostDelete([FromBody] List<int> ids)
        {
            if (ids == null || ids.Count == 0)
                return BadRequest(new { message = "No IDs provided" });

            var deletedIds = _itemService.DeleteItems(ids);

            return new JsonResult(new
            {
                message = $"Only delete the user without an order.",
                deletedIds
            });
        }
    }
}