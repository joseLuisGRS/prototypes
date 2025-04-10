﻿namespace Application.Services;

/// <summary>
/// This is the business rule used for role management.
/// </summary>
/// <param name="_roleRepository">The role repository is used for database operations. </param>
/// <param name="_mapper">The extension helps us to map other types of data. </param>
public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;
    private readonly ITokenExtractor _tokenExtractor;
    private UserDataTokenDto userDataTokenDto;

    public RoleService(IRoleRepository roleRepository, IMapper mapper, ITokenExtractor tokenExtractor)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
        _tokenExtractor = tokenExtractor;
        userDataTokenDto = _tokenExtractor.ExtractToken();
    }

    public async ValueTask<IEnumerable<RoleDto>> GetAllAsync()
    {
        var roles = await _roleRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<RoleDto>>(roles);
    }

    public async ValueTask<RoleDto> GetByIdAsync(Int64 id)
    {
        var role = await _roleRepository.GetByIdAsync(id);
        return _mapper.Map<RoleDto>(role);
    }

    public async ValueTask<RoleDto> CreateAsync(CreateRoleDto createRoleDto)
    {
        if (await _roleRepository.ExistsByNameAsync(createRoleDto.Name))
            throw new CustomException(SourceType.Application, SeverityType.Warning, EHttpStatusCode.BadRequest, "Role name already exists");

        var role = _mapper.Map<Role>(createRoleDto);
        role.CreationUserId = userDataTokenDto.Id;

        await _roleRepository.AddAsync(role);
        await _roleRepository.SaveChangesAsync();
        return _mapper.Map<RoleDto>(role);
    }

    public async ValueTask<bool> UpdateAsync(UpdateRoleDto updateRoleDto)
    {
        var role = await _roleRepository.GetByIdAsync(updateRoleDto.Id)
            ?? throw new CustomException(SourceType.Application, SeverityType.Warning, EHttpStatusCode.NotFound, "Role not found");

        role.Name = updateRoleDto.Name;
        role.Description = updateRoleDto.Description;
        role.ModificationUserId = userDataTokenDto.Id;

        _roleRepository.UpdateAsync(role);
        return await _roleRepository.SaveChangesAsync();
    }

    public async ValueTask<bool> UpdateStatusAsync(Int64 id)
    {
        var role = await _roleRepository.GetByIdAsync(id)
            ?? throw new CustomException(SourceType.Application, SeverityType.Warning, EHttpStatusCode.NotFound, "Role not found");

        role.ModificationUserId = userDataTokenDto.Id;

        _roleRepository.UpdateStatusAsync(role);
        return await _roleRepository.SaveChangesAsync();
    }

    public async ValueTask<bool> DeleteAsync(Int64 id)
    {
        var role = await _roleRepository.GetByIdAsync(id)
            ?? throw new CustomException(SourceType.Application, SeverityType.Warning, EHttpStatusCode.NotFound, "Role not found");

        role.ModificationUserId = userDataTokenDto.Id;

        _roleRepository.DeleteAsync(role);
        return await _roleRepository.SaveChangesAsync();
    }

    public async ValueTask<bool> ExistsByNameAsync(string name)
    {
        return await _roleRepository.ExistsByNameAsync(name);
    }
}
