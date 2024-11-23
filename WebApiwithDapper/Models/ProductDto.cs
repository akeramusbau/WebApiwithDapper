using System.ComponentModel.DataAnnotations;

namespace WebApiwithDapper.Models
{
    public class ProductDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        [Required, MaxLength(100)]
        public string Category { get; set; } = "";
        [Required, MaxLength(100)]
        public string Brand { get; set; } = "";
        [Required]
        public decimal Price { get; set; }

    }
}
