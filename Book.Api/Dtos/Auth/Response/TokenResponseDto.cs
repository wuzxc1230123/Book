namespace Book.Api.Dtos.Auth.Response;

public class TokenResponseDto
{
    public string Token { get; set; }=string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}
