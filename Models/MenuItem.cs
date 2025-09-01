using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RESTaurang.Models
{
    public class MenuItem
    {
        public int Id { get; set; }

        [Required, MaxLength(120)]
        public required string Name { get; set; }

        [Range(0, 1000000)]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        public bool IsPopular { get; set; }

        [MaxLength(500)]
        public string? ImageUrl { get; set; }
    }
}
