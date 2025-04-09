namespace Application.Mapping;

/// <summary>
/// Contains the mapping of entities and DTO to another data model.
/// </summary>
public static class AutoMapperProfiles
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<Role, RoleDto>.NewConfig();
        TypeAdapterConfig<Role, CreateRoleDto>.NewConfig();
        TypeAdapterConfig<Role, UpdateRoleDto>.NewConfig();
        TypeAdapterConfig<RoleDto, UpdateRoleDto>.NewConfig();

        TypeAdapterConfig<User, UserDto>.NewConfig()
            .Map(dto => dto.FirstName, src => src.Person.FirstName)
            .Map(dto => dto.LastName, src => src.Person.LastName)
            .Map(dto => dto.Role, src => src.Role.Name);
        TypeAdapterConfig<UserDto, UserRefreshTokenDto>.NewConfig();

    }

}