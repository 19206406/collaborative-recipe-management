using FluentValidation;

namespace User.API.Features.User.RegisterUser
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre es obligatorio")
                .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El correo es obligatorio")
                .EmailAddress().WithMessage("Esto no corresponde a un correo electronico");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("El password no puede ser vacío")
                .MinimumLength(4).WithMessage("El password debe de tener como minimo 4 caracteres"); 
        }
    }
}
