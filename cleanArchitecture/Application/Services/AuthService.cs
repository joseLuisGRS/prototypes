namespace Application.Services;

/// <summary>
/// This is the business rule used for authentication management.
/// </summary>
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IJwtTokenService _tokenService;
    private readonly ITokenExtractor _tokenExtractor;
    private UserDataTokenDto userDataTokenDto;

    public AuthService(IUserRepository userRepository, IMapper mapper, IJwtTokenService tokenService, ITokenExtractor tokenExtractor)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _tokenService = tokenService;
        _tokenExtractor = tokenExtractor;
        userDataTokenDto = _tokenExtractor.ExtractToken();
    }
    public async ValueTask<UserDto> LoginAsync(AuthenticateDto authenticateDto)
    {
        //Validate user exists
        var user = await _userRepository.GetByUserNameAsync(authenticateDto.User);

        if (user is null) return null;

        //Check the password is valid
        if (!_tokenService.IsValidPassword(authenticateDto.Password, user.PasswordSalt, user.PasswordHash)) return null;

        var userDto = _mapper.Map<UserDto>(user);

        //create token
        userDto.Token = _tokenService.CreateToken(userDto);

        //generate key to refresh token
        userDto.refreshKeyToken = _tokenService.GenerateRefreshToken();

        //save key to refresh token
        _tokenService.SaveKeyRefreshToken(userDto.UserName, userDto.refreshKeyToken);

        return userDto;
    }

    public async ValueTask<UserDto> RefreshAccessTokenAsync(RefreshTokenDto refreshTokenDto)
    {
        //Validate user exists
        var user = await _userRepository.GetByUserNameAsync(refreshTokenDto.User);

        if (user is null) return null;

        //validates data to refresh the token
        if (!_tokenService.ValidateRefreshToken(refreshTokenDto)) return null;

        var userDto = _mapper.Map<UserDto>(user);
        var userRefreshToken = _mapper.Map<UserRefreshTokenDto>(userDto);

        userRefreshToken.expiredToken = userDataTokenDto.Token;
        userRefreshToken.refreshKeyToken = refreshTokenDto.refreshKeyToken;

        //refresh the token
        userDto.Token = _tokenService.RefreshAccessToken(userRefreshToken);

        //generate key to refresh token
        userDto.refreshKeyToken = _tokenService.GenerateRefreshToken();

        //save key to refresh token
        _tokenService.SaveKeyRefreshToken(userDto.UserName, userDto.refreshKeyToken);

        return userDto;
    }

    public async ValueTask<bool> LogoutAsync()
    {
        string token = userDataTokenDto.Token;

        if (string.IsNullOrEmpty(token)) return false;

        //revoke token to evit future use
        _tokenService.RevokeToken(token);

        //clean the keys resfresh token to evit future use
        _tokenService.CleanKeyRefreshTokens(userDataTokenDto.UserName);
        return true;
    }
}
