using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.DTOs;
using WebApplication1.Services.Item;

namespace WebApplication1.Pages.Items
{
    public class EditModel : PageModel
    {
        private readonly IItemService _itemService;

        [BindProperty]
        public ItemDto.EditItem EditItem { get; set; } = new();

        public EditModel(IItemService itemService)
        {
            _itemService = itemService;
        }

        public IActionResult OnGet(int id)
        {
            var item = _itemService.GetItemById(id);
            if (item == null) return RedirectToPage("/Items/Index");

            EditItem = item;
            return Page();
        }

        public IActionResult OnPostItem()
        {
            if (!ModelState.IsValid) return Page();

            try
            {
                _itemService.UpdateItem(EditItem);
                return RedirectToPage("/Items/Index");
            }
            catch (KeyNotFoundException)
            {
                ModelState.AddModelError("", "Item not found.");
                return Page();
            }
        }
    }
}