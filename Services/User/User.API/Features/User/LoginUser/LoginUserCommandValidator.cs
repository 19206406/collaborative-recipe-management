using FluentValidation;

namespace User.API.Features.User.LoginUser
{
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El email no puede ser vacío")
                .EmailAddress().WithMessage("El valor no corresponde a un email");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("El password no puede ser vacío")
                .MinimumLength(4).WithMessage("El password debe de tener como minimo 4 caracteres"); 
        }
    }
}
