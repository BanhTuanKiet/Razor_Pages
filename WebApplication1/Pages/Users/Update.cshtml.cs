using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.DTOs;
using WebApplication1.Services.User;

namespace WebApplication1.Pages.Users
{
    public class UpdateModel : PageModel
    {
        private readonly IUserService _userService;

        public UpdateModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public UserDto.EditUser EditUser { get; set; } = new();

        // GET: /Users/Update/5
        public IActionResult OnGet(int id)
        {
            var user = _userService.GetUserById(id);

            if (user == null)
                return NotFound();

            // Bind dữ liệu từ DB vào form
            EditUser = new UserDto.EditUser
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone
            };

            return Page();
        }

        // POST: /Users/Update/5
        public IActionResult OnPostUser()
        {
            _userService.UpdateUser(EditUser);
            return RedirectToPage("/Users/Index");
        }
    }
}