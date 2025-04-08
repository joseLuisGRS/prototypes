namespace Application.Interfaces.Services;

/// <summary>
/// It is a contract that contains the business operations for the Authentication service.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Method to authenticate you if you have valid data. 
    /// </summary>
    /// <param name="authenticateDto">The authenticateDto parameter contains the user data.</param>
    /// <returns>Returns a UserDto in case the user data is validated successfully.</returns>
    ValueTask<UserDto> LoginAsync(AuthenticateDto authenticateDto);
    /// <summary>
    /// Method to refresh token if you have valid data.
    /// </summary>
    /// <param name="refreshTokenDto">The refreshTokenDto parameter contains the user data to refresh token.</param>
    /// <returns>Returns a UserDto in case the user data is validated successfully</returns>
    ValueTask<UserDto> RefreshAccessTokenAsync(RefreshTokenDto refreshTokenDto);
    /// <summary>
    /// Method to end session and remove the keys refresh token.
    /// </summary>
    /// <returns>Returns true on success or false otherwise.</returns>
    ValueTask<bool> LogoutAsync();
}