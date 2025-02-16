using System.ComponentModel.DataAnnotations;
namespace service_api.DTOs
{
    public class RegisterUserDto
    {
        [Required]
        public string Name { get; set; }

        [Required, StringLength(12, MinimumLength = 10)]
        public string NIC { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; } 
    }
}
