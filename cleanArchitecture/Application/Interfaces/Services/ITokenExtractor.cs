namespace Application.Interfaces.Services;

/// <summary>
/// It is a contract that contains the business operations for the token.
/// </summary>
public interface ITokenExtractor
{
    /// <summary>
    /// Extracts the token and converts it to UserDataTokenDto
    /// </summary>
    /// <returns>Returns UserDataTokenDto when the token is valid, otherwise returns null. </returns>
    public UserDataTokenDto ExtractToken();
}
