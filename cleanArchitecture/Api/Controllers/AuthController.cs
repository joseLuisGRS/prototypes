namespace Api.Controllers;

/// <summary>
/// Controller for handling auth related operations.
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[EnableRateLimiting("fixedWindow")]
public class AuthController(IAuthService _authService) : ControllerBase
{
    /// <summary>
    /// Login authentication.
    /// </summary>
    /// <param name="authenticateDto">The authenticateDto param contains the user data. </param>
    /// <response code="200">Returns UserDto</response>  
    /// <response code="400">Bad request</response>  
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Not found</response>
    /// <response code="500">Internal server error</response> 
    [HttpPost("login")]
    [AllowAnonymous]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async ValueTask<IActionResult> LogIn(AuthenticateDto authenticateDto)
    {
        var res = await _authService.LoginAsync(authenticateDto);

        return res is null
            ? Unauthorized(ApiResponse<string>.ErrorResponse("Invalid access."))
            : Ok(ApiResponse<UserDto>.SuccessResponse(res, "Access successfully."));
    }

    /// <summary>
    /// Generates the refresh token.
    /// </summary>
    /// <param name="refreshTokenDto">The refreshTokenDto param contains the user data.</param>
    /// <response code="200">Returns UserDto</response>  
    /// <response code="400">Bad request</response>  
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Not found</response>
    /// <response code="500">Internal server error</response>
    [HttpPost("refreshToken")]
    [Authorize]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async ValueTask<IActionResult> RefreshToken(RefreshTokenDto refreshTokenDto)
    {
        var res = await _authService.RefreshAccessTokenAsync(refreshTokenDto);

        return res is null
            ? Unauthorized(ApiResponse<string>.ErrorResponse("Invalid access."))
            : Ok(ApiResponse<UserDto>.SuccessResponse(res, "Refresh token successfully."));
    }

    /// <summary>
    /// Logout
    /// </summary>
    /// <response code="200">Returns a message indicating the session has been closed.</response>   
    /// <response code="401">Unauthorized</response>
    /// <response code="500">Internal server error</response>
    [HttpPost("logout")]
    [Authorize]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Logout()
    {
        var res = await _authService.LogoutAsync();

        return !res
            ? Unauthorized(ApiResponse<string>.ErrorResponse("Invalid access."))
            : Ok(ApiResponse<string>.SuccessResponse(null, "Closed session successfully."));
    }

}
