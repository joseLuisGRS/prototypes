namespace Application.DTOs;

public class CreateRoleDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Int64 CreationUserId { get; set; }
}
