using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.DTOs;
using WebApplication1.Services.Item;

namespace WebApplication1.Pages.Items
{
    public class AddModel : PageModel
    {
        private readonly IItemService _itemService;

        [BindProperty]
        public ItemDto.AddItem NewItem { get; set; } = new();

        public AddModel(IItemService itemService)
        {
            _itemService = itemService;
        }

        public void OnGet() { }

        public IActionResult OnPostItem()
        {
            if (!ModelState.IsValid) return Page();

            _itemService.AddItem(NewItem);
            return RedirectToPage("/Items/Index");
        }
    }
}