namespace Application.DTOs.Auth;

public class UserDataTokenDto
{
    public Int64 Id { get; set; }
    public string UserName { get; set; }
    public string Token { get; set; }
    public string Role { get; set; }
}
