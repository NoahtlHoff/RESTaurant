namespace RESTaurang.Dtos
{
    public record CustomerReadDto(int Id, string Name, string Phone);

    public class CustomerCreateDto
    {
        public string Name { get; set; } = default!;
        public string Phone { get; set; } = default!;
    }

    public class CustomerUpdateDto : CustomerCreateDto { }
}
