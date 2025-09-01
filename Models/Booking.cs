using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RESTaurang.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [NotMapped]
        public DateTime EndTime => StartTime.AddHours(2);

        [Range(1, 50)]
        public int Guests { get; set; }

        public int TableId_FK { get; set; }
        public Table Table { get; set; } = null!;

        public int CustomerId_FK { get; set; }
        public Customer Customer { get; set; } = null!;
    }
}
