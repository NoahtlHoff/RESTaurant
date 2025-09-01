namespace RESTaurang.Dtos
{
    public record BookingReadDto(int Id, DateTime StartTime, int Guests, int CustomerId, int TableId);

    public class BookingCreateDto
    {
        public DateTime StartTime { get; set; }
        public int Guests { get; set; }
        public int CustomerId_FK { get; set; }
        public int TableId_FK { get; set; }
    }

    public class BookingUpdateDto : BookingCreateDto { }
}
