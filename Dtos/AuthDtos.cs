namespace RESTaurang.Dtos
{
    public class AuthDtos
    {
        public class LoginRequestDto
        {
            public string Username { get; set; } = default!;
            public string Password { get; set; } = default!;
        }

        public record LoginResponseDto(string Token);
    }
}
