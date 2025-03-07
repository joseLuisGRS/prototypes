namespace Application.Validators;

/// <summary>
/// Class to validate data from CreateRoleDto
/// </summary>
public class CreateRoleValidator : AbstractValidator<CreateRoleDto>
{
    protected readonly IRoleRepository _roleRepository;

    public CreateRoleValidator(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;

        RuleFor(r => r.Name)
            .NotEmpty().WithMessage("El nombre del rol es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre del rol no puede exceder los 100 caracteres.");

        RuleFor(r => r.Description)
            .MaximumLength(150).WithMessage("La descripción no puede exceder los 150 caracteres.");
    }
}