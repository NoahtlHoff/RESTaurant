namespace RESTaurang.Dtos
{
    public record TableReadDto(int Id, string Number, int Capacity);
    public record TableAvailabilityDto(DateTime Start, DateTime End, List<TableReadDto> AvailableTables);
    public class TableCreateDto
    {
        public string Number { get; set; } = default!;
        public int Capacity { get; set; }
    }

    public class TableUpdateDto : TableCreateDto { }
}
