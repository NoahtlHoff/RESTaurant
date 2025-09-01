namespace RESTaurang.Dtos
{
    public record AdminReadDto(int Id, string Username);

    public class AdminCreateDto
    {
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
    }

    public class AdminUpdateDto : AdminCreateDto { }
}
