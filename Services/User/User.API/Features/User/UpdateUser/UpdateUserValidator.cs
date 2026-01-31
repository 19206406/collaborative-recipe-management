using FluentValidation;

namespace User.API.Features.User.UpdateUser
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre no puede ser vacío");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El email no puede ser vacío")
                .EmailAddress().WithMessage("El valor proporcionado no corresponde a un email");

            //RuleFor(x => x.IsActive)
            //    .NotEmpty().WithMessage("El campo de activación es requerido"); 
        }
    }
}
