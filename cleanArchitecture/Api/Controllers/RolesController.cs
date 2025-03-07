namespace Api.Controllers;

/// <summary>
/// Controller for handling role related operations.
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
//[Authorize]
public class RolesController(IRoleService _roleService, IMapper _mapper) : ControllerBase
{

    /// <summary>
    /// Get all roles
    /// </summary>
    /// <response code="200">Returns a list of roles (RoleDto)</response>  
    /// <response code="204">No roles found</response>  
    /// <response code="401">Unauthorized</response>
    /// <response code="500">Internal server error</response> 
    [HttpGet]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<IEnumerable<RoleDto>>>> GetAll() 
    {
        var roles = await _roleService.GetAllAsync();
        if (roles.Count() == 0) return NoContent();
        return Ok(ApiResponse<IEnumerable<RoleDto>>.SuccessResponse(roles, "Roles retrieved successfully"));
    }

    /// <summary>
    /// Get a role by id
    /// </summary>
    /// <param name="id"></param>
    /// <response code="200">Returns a role (RoleDto)</response>  
    /// <response code="401">Unauthorized</response>
    /// <response code="404">No role Found</response>
    /// <response code="500">Internal server error</response> 
    [HttpGet("{id}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RoleDto>> GetById(Int64 id)
    {
        var role = await _roleService.GetByIdAsync(id);
        if (role == null) return NotFound(ApiResponse<string>.ErrorResponse($"Role isn't found for id {id}"));
        return Ok(ApiResponse<RoleDto>.SuccessResponse(role, "Role retrieved successfully"));
       
    }

    /// <summary>
    /// Create a new role
    /// </summary>
    /// <param name="createRoleDto"></param>
    /// <returns></returns>
    /// <response code="200">Returns the newly created role (RoleDto)</response>  
    /// <response code="400">Bad Request</response>  
    /// <response code="401">Unauthorized</response>
    /// <response code="500">Internal server error</response> 
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RoleDto>> Create([FromBody] CreateRoleDto createRoleDto)
    {
        try
        {
            createRoleDto.CreationUserId = 1;

            var createdRole = await _roleService.CreateAsync(createRoleDto);
            return Created(nameof(GetById), ApiResponse<RoleDto>.SuccessResponse(createdRole, "Role created successfully."));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<string>.ErrorResponse(ex.Message));
        }
    }

    /// <summary>
    /// Update a role
    /// </summary>
    /// <param name="updateRoleDto"></param>
    /// <response code="200">Returns the updated role (RoleDto)</response>  
    /// <response code="400">Bad Request</response>  
    /// <response code="401">Unauthorized</response>
    /// <response code="404">No role found</response>
    /// <response code="500">Internal server error</response> 
    [HttpPut]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RoleDto>> Update([FromBody] UpdateRoleDto updateRoleDto)
    {
        try
        {
            updateRoleDto.ModificationUserId = 1;

            return await _roleService.UpdateAsync(updateRoleDto)
                ? Ok(ApiResponse<RoleDto>.SuccessResponse(_mapper.Map<RoleDto>(updateRoleDto), "Role updated with success!"))
                : BadRequest(ApiResponse<string>.ErrorResponse("Failed to update role"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Changes the status of the role 
    /// </summary>
    /// <param name="id"></param>
    /// <response code="200">Returns "Role updated successfully"</response>  
    /// <response code="400">Bad Request</response>  
    /// <response code="401">Unauthorized</response>
    /// <response code="404">No role found</response>
    /// <response code="500">Internal server error</response> 
    [HttpPatch("{id}/status")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<string>> UpdateStatus(Int64 id)
    {
        try
        {
            var userId = 1;

            await _roleService.UpdateStatusAsync(id, userId);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Role updated successfully."));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(ApiResponse<string>.ErrorResponse($"Role with ID {id} not found"));
        }
    }

    /// <summary>
    /// Delete a role
    /// </summary>
    /// <param name="id"></param>
    /// <response code="200">Returns "Role deleted successfully."</response>  
    /// <response code="400">Bad Request</response>  
    /// <response code="401">Unauthorized</response>
    /// <response code="404">No role found</response>
    /// <response code="500">Internal server error</response> 
    [HttpDelete("{id}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<string>> Delete(Int64 id)
    {
        try
        {
            var userId = 1;
            await _roleService.DeleteAsync(id, userId);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Role deleted successfully."));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(ApiResponse<string>.ErrorResponse($"Role with ID {id} not found"));
        }
    }

}
