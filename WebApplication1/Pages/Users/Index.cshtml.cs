using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.DTOs;
using WebApplication1.Services.User;

namespace WebApplication1.Pages.Users
{
    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;

        public IndexModel(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> OnPostUsers([FromBody] FilterDto.AgGridRequest filterRequest)
        {
            try
            {
                var users = await _userService.GetUsers(filterRequest);

                return new JsonResult(users);
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message, detail = ex.InnerException?.Message })
                {
                    StatusCode = 500
                };
            }
        }

        public IActionResult OnPostDelete([FromBody] List<int> ids)
        {
            if (ids == null || ids.Count == 0)
                return BadRequest(new { message = "No IDs provided" });

            var deletedIds = _userService.DeleteUsers(ids);

            return new JsonResult(new
            {
                message = $"Only delete the user without an order.",
                deletedIds
            });
        }
    }
}