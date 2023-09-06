using System.ComponentModel.DataAnnotations;

namespace ShellPG_Backend.Data.Models
{
    public class User
    {
        // Need to add more properties to this model
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string PasswordHash { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        // set default role as "User"
        public string Role { get; set; } = "User";

        // address
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        public string State { get; set; } = "KA";
        [Required]
        public string PinCode { get; set; }
        
    }
}
