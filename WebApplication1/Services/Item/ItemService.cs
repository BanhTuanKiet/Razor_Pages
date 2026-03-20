using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DTOs;
using WebApplication1.Helpers;
using WebApplication1.Models;
using WebApplication1.Services.Item;

namespace WebApplication1.Services
{
    public class ItemService : IItemService
    {
        private readonly AppDbContext _context;

        public ItemService(AppDbContext context)
        {
            _context = context;
        }

        public List<ItemDto.ItemBase> GetAll()
        {
            return _context.Items
                .Include(i => i.OrderDetails)
                .Select(i => new ItemDto.ItemBase
                {
                    Id = i.Id,
                    Name = i.Name,
                    Price = i.Price,
                    Stock = i.Stock,
                    CountSell = i.OrderDetails == null ? 0 : i.OrderDetails.Sum(o => o.Quantity)
                })
                .ToList();
        }

        public async Task<FilterDto.PagedResult<ItemDto.ItemBase>> GetItems(
            FilterDto.AgGridRequest request)
        {
            var query = _context.Items.AsQueryable();

            query = FilterHelper.ApplyFilters(query, request.Filters);
            query = FilterHelper.ApplySorts(query, request.Sorts);

            var totalCount = await query.CountAsync();

            var data = await query
                .Skip(request.Page * request.PageSize)
                .Take(request.PageSize)
                .Select(i => new ItemDto.ItemBase
                {
                    Id = i.Id,
                    Name = i.Name,
                    Price = i.Price,
                    Stock = i.Stock,
                    CountSell = i.OrderDetails == null ? 0 : i.OrderDetails.Sum(o => o.Quantity)
                })
                .ToListAsync();

            return new FilterDto.PagedResult<ItemDto.ItemBase>
            {
                Data = data,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / request.PageSize)
            };
        }
        public ItemDto.AddItem AddItem(ItemDto.AddItem dto)
        {
            var newItem = new Models.Item
            {
                Name = dto.Name,
                Price = dto.Price,
                Stock = dto.Stock
            };

            _context.Items.Add(newItem);
            _context.SaveChanges();

            return dto;
        }

        public ItemDto.EditItem? GetItemById(int id)
        {
            var item = _context.Items.FirstOrDefault(i => i.Id == id);
            if (item == null) return null;

            return new ItemDto.EditItem
            {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price,
                Stock = item.Stock,
            };
        }

        public ItemDto.EditItem UpdateItem(ItemDto.EditItem dto)
        {
            var item = _context.Items.FirstOrDefault(i => i.Id == dto.Id)
                ?? throw new KeyNotFoundException($"Item with id {dto.Id} not found");

            item.Name = dto.Name;
            item.Price = dto.Price;
            item.Stock = dto.Stock;

            _context.SaveChanges();

            return dto;
        }

        public List<int> DeleteItems(List<int> ids)
        {
            var items = _context.Items
                .Where(i => ids.Contains(i.Id))
                .ToList();

            if (items.Count == 0)
                throw new KeyNotFoundException("No items found with the provided IDs");

            _context.Items.RemoveRange(items);
            _context.SaveChanges();

            return items.Select(i => i.Id).ToList();
        }
    }
}