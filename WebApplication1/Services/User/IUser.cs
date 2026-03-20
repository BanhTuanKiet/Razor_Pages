using System.Collections.Generic;
using WebApplication1.DTOs;

namespace WebApplication1.Services.User
{
    public interface IUserService
    {
        Task<FilterDto.PagedResult<UserDto.UserBase>> GetUsers(FilterDto.AgGridRequest filterRequest);
        UserDto.AddUser AddUser(UserDto.AddUser user);
        UserDto.EditUser? GetUserById(string id);
        UserDto.EditUser UpdateUser(UserDto.EditUser user);
        List<string> DeleteUsers(List<string> ids);
    }
}