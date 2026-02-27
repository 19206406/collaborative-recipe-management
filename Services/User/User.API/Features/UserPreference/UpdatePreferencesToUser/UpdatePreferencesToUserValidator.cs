using FluentValidation;

namespace User.API.Features.UserPreference.UpdatePreferencesToUser
{
    public class UpdatePreferencesToUserValidator : AbstractValidator<UpdatePreferencesToUserCommand>
    {
        public UpdatePreferencesToUserValidator()
        {
            RuleFor(x => x.Preferences)
                .NotEmpty().WithMessage("Debes de especificar las preferencias"); 
        }
    }
}
