using System.Collections.Generic;
using WebApplication1.DTOs;

namespace WebApplication1.Services.User
{
    public interface IUserService
    {
        List<UserDto.UserBase> GetUsers();
        UserDto.AddUser AddUser(UserDto.AddUser user);
        UserDto.EditUser? GetUserById(int id);
        UserDto.EditUser UpdateUser(UserDto.EditUser user);
        List<int> DeleteUsers(List<int> ids);
    }
}