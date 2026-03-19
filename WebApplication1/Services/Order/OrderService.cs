using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DTOs;
using WebApplication1.Helpers;
using WebApplication1.Services.Item;
using WebApplication1.Services.Order;

namespace WebApplication1.Services
{
    public class OrderService : IOrderServcie
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public List<OrderDto.OrderBase> GetAll()
        {
            return _context.Orders
                .Select(o => new OrderDto.OrderBase
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate.ToString("yyyy-MM-dd"),
                    User = null
                })
                .ToList();
        }

        public async Task<FilterDto.PagedResult<OrderDto.OrderBase>> GetOrders(
            FilterDto.AgGridRequest request,
            int? userId = null)
        {
            var query = _context.Orders
                .Include(o => o.User)
                .AsQueryable();

            if (userId.HasValue)
                query = query.Where(o => o.UserId == userId.Value);

            query = FilterHelper.ApplyFilters(query, request.Filters);
            query = FilterHelper.ApplySorts(query, request.Sorts);

            var totalCount = await query.CountAsync();

            var data = await query
                .Skip(request.Page * request.PageSize)
                .Take(request.PageSize)
                .Select(o => new OrderDto.OrderBase
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate.ToString("yyyy-MM-dd"),
                    User = o.User == null ? null : new UserDto.UserBase
                    {
                        Id = o.User.Id,
                        Name = o.User.Name,
                        Email = o.User.Email,
                        Phone = o.User.Phone
                    }
                })
                .ToListAsync();

            return new FilterDto.PagedResult<OrderDto.OrderBase>
            {
                Data = data,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / request.PageSize)
            };
        }
    }
}