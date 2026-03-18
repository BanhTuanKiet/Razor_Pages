using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DTOs;
using WebApplication1.Services.User;

namespace WebApplication1.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public List<UserDto.UserBase> GetUsers()
        {
            return _context.Users
                .Select(u => new UserDto.UserBase
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    Phone = u.Phone
                })
                .ToList();
        }

        public UserDto.AddUser AddUser(UserDto.AddUser user)
        {
            var newUser = new Models.User
            {
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return user;
        }

        public UserDto.EditUser? GetUserById(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);

            if (user == null) return null;

            return new UserDto.EditUser
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone
            };
        }

        public UserDto.EditUser UpdateUser(UserDto.EditUser dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == dto.Id)
                ?? throw new KeyNotFoundException($"User with id {dto.Id} not found");

            user.Name = dto.Name;
            user.Email = dto.Email;
            user.Phone = dto.Phone;

            _context.SaveChanges();

            return dto;
        }

        public List<int> DeleteUsers(List<int> ids)
        {
            var users = _context.Users
                .Where(u => ids.Contains(u.Id))
                .Include(u => u.Orders)
                .ToList();

            if (users.Count == 0)
                throw new KeyNotFoundException("No users found with the provided IDs");

            var usersToDelete = users
                .Where(u => u.Orders == null || u.Orders.Count() <= 0)
                .ToList();

            _context.Users.RemoveRange(usersToDelete);
            _context.SaveChanges();
        
            return usersToDelete.Select(u => u.Id).ToList();
        }
    }
}