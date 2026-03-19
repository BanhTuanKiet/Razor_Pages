using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs
{
    public class ItemDto
    {
        public class ItemBase
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public int CountSell { get; set; } = 0;
            public decimal? Price { get; set; }
            public int? Stock { get; set; }
        }

        public class AddItem
        {
            [Required(ErrorMessage = "Name cannot be blank")]
            [StringLength(100, ErrorMessage = "Name must be at most 100 characters")]
            public string Name { get; set; } = string.Empty;

            [Required(ErrorMessage = "Price is required")]
            [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
            public decimal? Price { get; set; }

            [Required(ErrorMessage = "Stock is required")]
            [Range(0, int.MaxValue, ErrorMessage = "Stock must be >= 0")]
            public int? Stock { get; set; }
        }

        public class EditItem : AddItem
        {
            [Required(ErrorMessage = "Id is required")]
            public int Id { get; set; }
        }
    }
}