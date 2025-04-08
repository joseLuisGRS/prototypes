namespace Application.Validators.Auth;

/// <summary>
/// Class to validate data from AuthenticateDto
/// </summary>
public class AuthenticateValidator : AbstractValidator<AuthenticateDto>
{

    public AuthenticateValidator()
    {
        RuleFor(r => r.User)
            .NotEmpty().WithMessage("El Usuario es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre del usuario no puede exceder los 100 caracteres.");

        RuleFor(r => r.Password)
            .NotEmpty().WithMessage("El password es obligatorio.")
            .MaximumLength(150).WithMessage("El password no puede exceder los 150 caracteres.");
    }

}
