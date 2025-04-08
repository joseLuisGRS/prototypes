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

        CreateMap<User, UserDto>()
            .ForMember(dto => dto.FirstName, e => e.MapFrom(src => src.Person.FirstName))
            .ForMember(dto => dto.LastName, e => e.MapFrom(src => src.Person.LastName))
            .ForMember(dto => dto.Role, e => e.MapFrom(src => src.Role.Name))
            .ReverseMap();
        CreateMap<UserDto, UserRefreshTokenDto>().ReverseMap();
    }
}