using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShellPG_Backend.Data.Model
{
    [Table("Products", Schema = "product_schema")]

    public class Product
    {
        public int Id { get; set; }

        [Required]
        [Url]
        [StringLength(500)] 
        public string ImageUrl { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Range(1, 5)] 
        public decimal Rating { get; set; }


    }

}
