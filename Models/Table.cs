using System.ComponentModel.DataAnnotations;

namespace RESTaurang.Models
{
    public class Table
    {
        public int Id { get; set; }

        [Required, Range(1, 100)]
        public int Capacity { get; set; }

        [Required, MaxLength(20)]
        public required string Number { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
