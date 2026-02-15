using FluentValidation;

namespace Rating.API.Features.Rating.CreateRating
{
    public class CreateRatingCommandValidator : AbstractValidator<CreateRatingCommand>
    {
        public CreateRatingCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("El Id del usuario no puede ser vacio");

            RuleFor(x => x.RecipeId)
                .NotEmpty().WithMessage("El Id de la receta no puede ser vacio");

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5).WithMessage("La calificación debe estar entre 1 y 5")
                .NotEmpty().WithMessage("La calificación no puede ser vacia"); 

            RuleFor(x => x.Comment)
                .MaximumLength(500).WithMessage("El comentario no puede superar los 500 caracteres");
        }
    }
}
