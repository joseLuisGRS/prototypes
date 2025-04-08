namespace Application.Interfaces.Services;

/// <summary>
/// It is a contract that contains the business operations for the role repository.
/// </summary>
public interface IRoleService
{
    /// <summary>
    /// Get all data of roles
    /// </summary>
    /// <returns>Returns a list of roles with a RoleDto output mapping. </returns>
    ValueTask<IEnumerable<RoleDto>> GetAllAsync();
    /// <summary>
    /// Get the data of roles by Id.
    /// </summary>
    /// <param name="id">The Id property is the parameter for the search. </param>
    /// <returns>Returns the roles entity with an output mapping RoleDto. </returns>
    ValueTask<RoleDto> GetByIdAsync(Int64 id);
    /// <summary>
    /// Creates a new Roles in the database.
    /// </summary>
    /// <param name="createRoleDto">>The createRoleDto parameter contains the role data. </param>
    /// <returns>Returns the new role created with a RoleDto output mapping. </returns>
    /// <returns>Returns a CustomException in case the role name already exists. </returns>
    ValueTask<RoleDto> CreateAsync(CreateRoleDto createRoleDto);
    /// <summary>
    /// Updates the data of an existing role.
    /// </summary>
    /// <param name="updateRoleDto">The updateRoleDto parameter contains the role data. </param>
    /// <returns>Returns true when the role was successfully updated or false otherwise. </returns>
    /// <returns>Returns a CustomException in case the role name already exists. </returns>
    ValueTask<bool> UpdateAsync(UpdateRoleDto updateRoleDto);
    /// <summary>
    /// Updates the status of an existing role.
    /// </summary>
    /// <param name="id">The id parameter is used to search the role. </param>
    /// <returns>Returns true when the role was successfully updated or false otherwise. </returns>
    /// <returns>Returns a CustomException in case the role name already exists. </returns>
    ValueTask<bool> UpdateStatusAsync(Int64 id);
    /// <summary>
    /// Deletes an existing role.
    /// </summary>
    /// <param name="id">The id parameter is used to search the role.</param>
    /// <returns>Returns true when the role was successfully updated or false otherwise. </returns>
    /// <returns>Returns a CustomException in case the role name already exists. </returns>
    ValueTask<bool> DeleteAsync(Int64 id);
    /// <summary>
    /// Validates the existence of a role by its name.
    /// </summary>
    /// <param name="name">The name parameter is used to search the role. </param>
    /// <returns>Returns true when the role was found or false otherwise.</returns>
    ValueTask<bool> ExistsByNameAsync(string name);
}
