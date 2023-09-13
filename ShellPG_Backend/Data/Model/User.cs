using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShellPG_Backend.Data.Model
{
    [Table("Users")]
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

        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(500)] 
        public string Address { get; set; }

        [Required]
        [StringLength(255)] 
        public string Password { get; set; }
        //public ICollection<Order> Orders { get; set; } = new List<Order>();

    }

}
