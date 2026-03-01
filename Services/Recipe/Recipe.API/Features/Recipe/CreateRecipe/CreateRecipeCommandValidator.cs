using FluentValidation;

namespace Recipe.API.Features.Recipe.CreateRecipe
{
    public class CreateRecipeCommandValidator : AbstractValidator<CreateRecipeCommand>
    {
        public CreateRecipeCommandValidator()
        {
            RuleFor(x => x.Recipe.Title)
                .NotEmpty().WithMessage("El titulo de la receta no puede ser vacio")
                .MaximumLength(200).WithMessage("El titulo no puede superar los 200 caracteres");

            RuleFor(x => x.Recipe.Description)
                .NotEmpty().WithMessage("La descripción de la receta no puede ser vacia");

            RuleFor(x => x.Recipe.PrepTimeMinutes)
                .NotEmpty().WithMessage("debes de establecer un tiempo de preparación en minutos");

            RuleFor(x => x.Recipe.CookTimeMinutes)
                .NotEmpty().WithMessage("debes de establecer un tiempo de coción en minutos");

            RuleFor(x => x.Recipe.Difficulty)
                 .NotEmpty().WithMessage("Debes de establecer la dificultad de la receta")
                 .MaximumLength(40).WithMessage("La especificación de la dificulta no puede superar los cuarenta caracteres"); 
                
            RuleFor(x => x.Recipe.Servings)
                .NotEmpty().WithMessage("Debes de establecer el numero de porciones de la receta");



            RuleFor(x => x.Ingredients)
                .NotEmpty().WithMessage("La receta debe tener una lista de ingredientes")
                .Must(x => x.Count >= 3).WithMessage("La receta debe de tener como minimo 3 ingredientes");

            RuleForEach(x => x.Ingredients)
                .ChildRules(ing =>
                {
                    ing.RuleFor(x => x.Name)
                        .NotEmpty().WithMessage("El nombre del ingrediente no puede ser vacio")
                        .MaximumLength(200).WithMessage("El nombre del ingrediente no puede superar los 200 caracteres");

                    ing.RuleFor(x => x.Quantity)
                        .NotEmpty().WithMessage("La cantidad del ingrediente es requeridad");

                    ing.RuleFor(x => x.Unit)
                        .NotEmpty().WithMessage("La unidad de medida del ingrediente es requerida")
                        .MaximumLength(50).WithMessage("La especificación de unidad de medida no puede superar los 50 caracteres");

                    ing.RuleFor(x => x.DisplayOrder)
                        .NotEmpty().WithMessage("La orden de visualización no puede ser vacio");
                });



            RuleFor(x => x.Steps)
                .NotEmpty().WithMessage("La receta debe de tener una serie de pasos")
                .Must(x => x.Count >= 3).WithMessage("La receta debe de tener como minimo 3 pasos");

            RuleForEach(x => x.Steps)
                .ChildRules(step =>
                {
                    step.RuleFor(s => s.Instruction)
                        .NotEmpty().WithMessage("La instrucción de un paso no puede ser vacia");
                });
        }
    }
}
