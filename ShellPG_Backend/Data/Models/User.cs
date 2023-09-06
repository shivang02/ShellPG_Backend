using System.ComponentModel.DataAnnotations;

namespace ShellPG_Backend.Data.Models
{
    public class User
    {
        // Need to add more properties to this model
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string PasswordHash { get; set; }
    }
}
