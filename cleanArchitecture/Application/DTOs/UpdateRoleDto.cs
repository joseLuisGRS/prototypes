namespace Application.DTOs;

public class UpdateRoleDto
{
    public Int64 Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Int64 ModificationUserId { get; set; }
}
