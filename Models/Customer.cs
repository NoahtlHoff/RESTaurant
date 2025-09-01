using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace RESTaurang.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public required string Name { get; set; }

        [Required, MaxLength(30)]
        public required string Phone { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
