using System.ComponentModel.DataAnnotations;

namespace ShellPG_Backend.Data.Models
{
    public class User: IDInterface
    {
        // use IDInterface
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

        // address model
        public string AddressLine1 { get; set; } = "";
        public string AddressLine2 { get; set; } = "";
        public string City { get; set; } = "";
        public string State { get; set; } = "";
        public string PinCode { get; set; } = "";

    }
}
