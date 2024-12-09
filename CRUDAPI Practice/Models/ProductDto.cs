using System.ComponentModel.DataAnnotations;

namespace CRUDAPI_Practice.Models
{
    //This will store data from the user
    public class ProductDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Brand { get; set; } = string.Empty;

        [Required]
        public string Category { get; set; } = string.Empty;

        [Required]
        public Decimal Price { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;
    }
}
