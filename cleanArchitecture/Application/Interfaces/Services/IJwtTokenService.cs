namespace Application.Interfaces.Services;

/// <summary>
/// It is a contract that contains the business operations for the jwt service.
/// </summary>
public interface IJwtTokenService
{
    /// <summary>
    /// This method creates a new token for the requesting user.
    /// </summary>
    /// <param name="userDto">The userDto parameter contains the user data.</param>
    /// <returns>Returns a UserDto.</returns>
    string CreateToken(UserDto userDto);
    /// <summary>
    /// This method provides the hash and salt of the password.
    /// </summary>
    /// <param name="password">The password is the property that is used to process the hash and salt.</param>
    /// <returns>Returns a new property of HmacSha.</returns>
    HmacSha GetHmacHashAndSalt(string password);
    /// <summary>
    /// Validates the user's password.
    /// </summary>
    /// <param name="password">The password is the property is used to compare with the passwordHash.</param>
    /// <param name="passwordSalt">The password is the property is used to generate hmac.</param>
    /// <param name="passwordHash">The passwordHash is the property is used to compare with the password.</param>
    /// <returns>Returns true if the password is valid or false otherwise.</returns>
    bool IsValidPassword(string password, byte[] passwordSalt, byte[] passwordHash);
    /// <summary>
    /// Generates a key to refresh token.
    /// </summary>
    /// <returns>A Key generated to refresh token.</returns>
    string GenerateRefreshToken();
    /// <summary>
    /// Saves the Key that is used for a refresh token.
    /// </summary>
    /// <param name="userId">The userId param represents the user's id. </param>
    /// <param name="refreshToken">The refreshToken param is a value that is saved for later use.</param>
    void SaveKeyRefreshToken(string userId, string refreshToken);
    /// <summary>
    /// Verify that the user data is valid to update the token.
    /// </summary>
    /// <param name="refreshTokenDto">The refreshTokenDto param contains the user data.</param>
    /// <returns>Returns true if the user data is valid or false otherwise.</returns>
    bool ValidateRefreshToken(RefreshTokenDto refreshTokenDto);
    /// <summary>
    /// Generates the refresh token and deletes the expired token and key refresh token.
    /// </summary>
    /// <param name="userRefreshTokenDto">The userRefreshTokenDto param contains the user data.</param>
    /// <returns>Returns the new token.</returns>
    string RefreshAccessToken(UserRefreshTokenDto userRefreshTokenDto);
    /// <summary>
    /// Revoke the token to evit its use and delete expired tokens.
    /// </summary>
    /// <param name="token">The token parameter is used to revoke the token.</param>
    /// <param name="additionalExpireMinutes">The additionalExpireMinutes param is the time added to revoke the token 
    /// (If the value is equal to 0, the time defined in the settings is used). </param>
    void RevokeToken(string token, int additionalExpireMinutes = 0);
    /// <summary>
    /// Deletes the keys refresh token for a specific user.
    /// </summary>
    /// <param name="userName">The userName param is the value used to delete the keys.</param>
    void CleanKeyRefreshTokens(string userName);
    /// <summary>
    /// Verify that the token exists in the token revocation list.
    /// </summary>
    /// <param name="token">The token parameter is used to verify that the token exists. </param>
    /// <returns>Returns true if the token exists or false otherwise.</returns>
    ValueTask<bool> IsTokenRevokedAsync(string token);
}
