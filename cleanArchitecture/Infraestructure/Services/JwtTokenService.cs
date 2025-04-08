namespace Infrastructure.Services;

/// <summary>
/// Provides methods used for jwt operations.
/// </summary>
public class JwtTokenService : IJwtTokenService
{
    private readonly JwtSettings _jwtSettings;
    private readonly ConcurrentDictionary<string, DateTime> _revokedTokens;
    private readonly ConcurrentDictionary<string, string> _refreshTokens;
    private readonly IMapper _mapper;

    public JwtTokenService(IOptions<JwtSettings> options, IMapper mapper)
    {
        _jwtSettings = options.Value;
        _mapper = mapper;
        _revokedTokens = new ConcurrentDictionary<string, DateTime>();
        _refreshTokens = new ConcurrentDictionary<string, string>();
    }

    public string CreateToken(UserDto user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName + "." + user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var jwtSecrets = Environment.GetEnvironmentVariable(_jwtSettings?.Secrets!);

        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(jwtSecrets));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpireMinutes),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public HmacSha GetHmacHashAndSalt(string password)
    {
        using var hmac = new HMACSHA512();
        return new HmacSha()
        {
            ComputeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
            Key = hmac.Key
        };
    }

    public bool IsValidPassword(string password, byte[] passwordSalt, byte[] passwordHash)
    {
        using var hmac = new HMACSHA512(passwordSalt);
        return hmac.ComputeHash(Encoding.UTF8.GetBytes(password)).Compare(passwordHash);
    }

    public string GenerateRefreshToken()
    {
        var refreshToken = Guid.NewGuid().ToString();
        return refreshToken;
    }

    public void SaveKeyRefreshToken(string userId, string refreshToken)
    {
        _refreshTokens[refreshToken] = userId;
    }

    public bool ValidateRefreshToken(RefreshTokenDto refreshTokenDto)
    {
        if (_refreshTokens.ContainsKey(refreshTokenDto.refreshKeyToken))
        {
            var userName = _refreshTokens[refreshTokenDto.refreshKeyToken];

            if (userName != refreshTokenDto.User) return false;

            return true;
        }

        return false;
    }

    public string RefreshAccessToken(UserRefreshTokenDto userRefreshTokenDto)
    {

        try
        {
            RevokeToken(userRefreshTokenDto.expiredToken);
            var user = _mapper.Map<UserDto>(userRefreshTokenDto);
            _refreshTokens.TryRemove(userRefreshTokenDto.refreshKeyToken, out _);
            return CreateToken(user);
        }
        catch
        {
            throw new InvalidOperationException("The token could not be updated.");
        }
    }

    public void RevokeToken(string token, int additionalExpireMinutes = 0)
    {
        if (additionalExpireMinutes == 0) additionalExpireMinutes = _jwtSettings.AdditionalExpireMinutes;
        _revokedTokens[token] = DateTime.UtcNow.AddMinutes(additionalExpireMinutes);

        CleanExpiredRevokedTokens();
    }

    private void CleanExpiredRevokedTokens()
    {
        var expiredTokens = _revokedTokens
            .Where(x => x.Value < DateTime.UtcNow)
            .Select(x => x.Key)
            .ToList();

        foreach (var token in expiredTokens)
        {
            _revokedTokens.TryRemove(token, out _);
        }
    }

    public ValueTask<bool> IsTokenRevokedAsync(string token)
    {
        return ValueTask.FromResult(_revokedTokens.ContainsKey(token));
    }

    public void CleanKeyRefreshTokens(string userName)
    {
        var keyRefresTokens = _refreshTokens
           .Where(x => x.Value == userName)
           .Select(x => x.Key)
           .ToList();

        foreach (var token in keyRefresTokens)
        {
            _refreshTokens.TryRemove(token, out _);
        }
    }

    /// <summary>
    /// Debugging protection method
    /// </summary>
    [System.Diagnostics.Conditional("RELEASE")]
    private void AntiDebuggingCheck()
    {
        if (System.Diagnostics.Debugger.IsAttached)
        {
            Environment.FailFast("Debugger detected. Stopping application.");
        }
    }

}
