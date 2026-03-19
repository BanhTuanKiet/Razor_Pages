using System.Collections.Generic;
using WebApplication1.DTOs;

namespace WebApplication1.Services.Item
{
    public interface IItemService
    {
        List<ItemDto.ItemBase> GetAll();
        Task<FilterDto.PagedResult<ItemDto.ItemBase>> GetItems(FilterDto.AgGridRequest request);
        ItemDto.AddItem AddItem(ItemDto.AddItem dto);
        ItemDto.EditItem? GetItemById(int id);
        ItemDto.EditItem UpdateItem(ItemDto.EditItem dto);
        List<int> DeleteItems(List<int> ids);
    }
}