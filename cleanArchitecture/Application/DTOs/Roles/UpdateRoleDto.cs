namespace Application.DTOs.Roles;

public class UpdateRoleDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public long ModificationUserId { get; set; }
}
