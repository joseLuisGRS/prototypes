namespace Application.Validators.Auth;

/// <summary>
/// Class to validate data from RefreshTokenDto
/// </summary>
public class RefreshTokenValidator : AbstractValidator<RefreshTokenDto>
{

    public RefreshTokenValidator()
    {
        RuleFor(r => r.User)
            .NotEmpty().WithMessage("El Usuario es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre del usuario no puede exceder los 100 caracteres.");

        RuleFor(r => r.refreshKeyToken)
            .NotEmpty().WithMessage("El campo refreshKeyToken es obligatorio.")
            .MaximumLength(150).WithMessage("El refreshKeyToken no puede exceder los 150 caracteres.");
    }

}
