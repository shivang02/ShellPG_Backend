using System.ComponentModel.DataAnnotations;

namespace ShellPG_Backend.Data.Model
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        [Phone]
        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(500)] 
        public string Address { get; set; }

        [Required]
        [StringLength(255)] 
        public string Password { get; set; }
    }

}
