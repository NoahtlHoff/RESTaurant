using System.ComponentModel.DataAnnotations;

namespace RESTaurang.Models
{
    public class Admin
    {
        public int Id { get; set; }

        [Required, MaxLength(60)]
        public required string Username { get; set; }

        [Required]
        public required byte[] PasswordHash { get; set; }

        [Required]
        public required byte[] PasswordSalt { get; set; }
    }
}
