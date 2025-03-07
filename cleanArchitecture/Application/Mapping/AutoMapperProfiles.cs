namespace Application.Mapping;

/// <summary>
/// Contains the mapping of entities and DTO to another data model.
/// </summary>
public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Role, RoleDto>().ReverseMap();
        CreateMap<Role, CreateRoleDto>().ReverseMap();
        CreateMap<Role, UpdateRoleDto>().ReverseMap();
        CreateMap<RoleDto, UpdateRoleDto>().ReverseMap();
    }
}