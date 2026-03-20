using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DTOs;
using WebApplication1.Helpers;
using WebApplication1.Models;
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

        public async Task<FilterDto.PagedResult<UserDto.UserBase>> GetUsers(FilterDto.AgGridRequest agGridRequest)
        {
            var query = _context.Users.AsQueryable();

            query = FilterHelper.ApplyFilters(query, agGridRequest.Filters);

             var totalCount = await query.CountAsync();

            var data = await query
                .Skip(agGridRequest.Page * agGridRequest.PageSize)
                .Take(agGridRequest.PageSize)
                .Select(u => new UserDto.UserBase
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email!,
                    PhoneNumber = u.PhoneNumber!
                })
                .ToListAsync();

            return new FilterDto.PagedResult<UserDto.UserBase>
            {
                Data = data,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / agGridRequest.PageSize)
            };
        }

        public UserDto.AddUser AddUser(UserDto.AddUser user)
        {
            var newUser = new Models.ApplicationUser
            {
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return user;
        }

        public UserDto.EditUser? GetUserById(string id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);

            if (user == null) return null;

            return new UserDto.EditUser
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email!,
                PhoneNumber = user.PhoneNumber!
            };
        }

        public UserDto.EditUser UpdateUser(UserDto.EditUser dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == dto.Id)
                ?? throw new KeyNotFoundException($"User with id {dto.Id} not found");

            user.Name = dto.Name;
            user.Email = dto.Email;
            user.PhoneNumber = dto.PhoneNumber;

            _context.SaveChanges();

            return dto;
        }

        public List<string> DeleteUsers(List<string> ids)
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