using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.DTOs;
using WebApplication1.Services.User;

namespace WebApplication1.Pages.Users
{
    public class AddModel : PageModel
    {
        private readonly IUserService _userService;

        [BindProperty]
        public UserDto.AddUser NewUser { get; set; } = new();

        public AddModel(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult OnPostUser()
        {
            _userService.AddUser(NewUser);

            return RedirectToPage("/Users/Index");
        }

    }
}