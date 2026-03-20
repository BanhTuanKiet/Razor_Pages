using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs
{
    public class UserDto
    {
        public class UserBase
        {
            public string Id { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string PhoneNumber { get; set; } = string.Empty;
        }

        public class AddUser
        {
            [Required(ErrorMessage = "Name cannot be blank")]
            [StringLength(100, ErrorMessage = "Name must be 100 characters long")]
            public string Name { get; set; } = string.Empty;

            [Required(ErrorMessage = "Email cannot be blank")]
            [EmailAddress(ErrorMessage = "Invalid email address")]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "Phone number cannot be blank")]
            [RegularExpression(@"^(0|\+84)[0-9]{9}$", ErrorMessage = "Invalid phone number")]
            public string PhoneNumber { get; set; } = string.Empty;
        }

        public class EditUser : AddUser
        {
            [Required(ErrorMessage = "Id is required")]
            public string Id { get; set; } = string.Empty;
        }
    }
}