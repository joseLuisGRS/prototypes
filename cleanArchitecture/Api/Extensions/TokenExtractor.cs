namespace Api.Extensions;

/// <summary>
/// This is the business rule used for the token.
/// </summary>
/// <param name="httpContextAccessor">The httpContextAccessor param is used for the current request.</param>
public class TokenExtractor(IHttpContextAccessor httpContextAccessor) : ITokenExtractor
{
    public UserDataTokenDto ExtractToken()
    {
        UserDataTokenDto userDataTokenDto;

        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext == null) return null;

        var authorizationHeader = httpContext.Request.Headers["Authorization"].ToString();
        if (string.IsNullOrEmpty(authorizationHeader)) return null;

        if (authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            string token = authorizationHeader.Substring("Bearer ".Length).Trim();
            var subject = "";
            var role = "";

            var tokenHandler = new JwtSecurityTokenHandler();
            var decodedToken = tokenHandler.ReadJwtToken(token);
            if (decodedToken != null)
            {
                subject = decodedToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
                role = decodedToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
            }

            string[] userData = subject.Split(".");
            string userName = userData[0];
            Int64 id = Convert.ToInt64(userData[1]);

            return userDataTokenDto = new UserDataTokenDto()
            {
                Id = id,
                UserName = userName,
                Token = token,
                Role = role
            };
        }

        return null;
    }
}
