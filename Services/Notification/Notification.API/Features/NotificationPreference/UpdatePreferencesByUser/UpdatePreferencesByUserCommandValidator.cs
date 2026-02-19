using FluentValidation;

namespace Notification.API.Features.NotificationPreference.UpdatePreferencesByUser
{
    public class UpdatePreferencesByUserCommandValidator : AbstractValidator<UpdatePreferencesByUserCommand>
    {
        public UpdatePreferencesByUserCommandValidator()
        {
            RuleFor(x => x.NotificationPreferences)
                .NotEmpty()
                .WithMessage(""); 
        }
    }
}
