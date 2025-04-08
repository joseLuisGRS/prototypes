namespace Application.DTOs.Auth;

public class RefreshTokenDto
{
    public string User { get; set; }

    public string refreshKeyToken { get; set; }
}
