using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs
{
    public class OrderDto
    {
        public class OrderBase
        {
            public int Id { get; set; }
            public string OrderDate { get; set; } = string.Empty;
            public UserDto.UserBase? User { get; set; }
        }
    }
}