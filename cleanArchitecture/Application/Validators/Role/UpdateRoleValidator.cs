namespace Application.Validators.Role;

/// <summary>
/// Class to validate data from UpdateRoleDto
/// </summary>
public class UpdateRoleValidator : AbstractValidator<UpdateRoleDto>
{
    protected readonly IRoleRepository _roleRepository;

    public UpdateRoleValidator(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;

        RuleFor(r => r.Id)
            .NotEmpty().WithMessage("El id del rol es obligatorio")
            .GreaterThan(0).WithMessage("El id del rol debe ser número entero positivo.");

        RuleFor(r => r.Name)
            .NotEmpty().WithMessage("El nombre del rol es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre del rol no puede exceder los 100 caracteres.");

        RuleFor(r => r.Description)
            .MaximumLength(150).WithMessage("La descripción no puede exceder los 150 caracteres.");
    }
}