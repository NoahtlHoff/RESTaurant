namespace RESTaurang.Dtos
{
    public record MenuItemReadDto(int Id, string Name, decimal Price, string? Description, bool IsPopular, string? ImageUrl);

    public class MenuItemCreateDto
    {
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public bool IsPopular { get; set; }
        public string? ImageUrl { get; set; }
    }

    public class MenuItemUpdateDto : MenuItemCreateDto { }
}
