using System.ComponentModel.DataAnnotations;
namespace service_api.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, StringLength(12, MinimumLength = 10)]
        public string NIC { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(6)]
        public string PasswordHash { get; set; }  
    }
}
