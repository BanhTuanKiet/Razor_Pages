using System.Collections.Generic;
using WebApplication1.DTOs;

namespace WebApplication1.Services.Order
{
    public interface IOrderServcie
    {
        List<OrderDto.OrderBase> GetAll();
        Task<FilterDto.PagedResult<OrderDto.OrderBase>> GetOrders(FilterDto.AgGridRequest request, string? userId = null);
    }
}