using FastEndpoints;
using MediatR;

namespace Rating.API.Features.Rating.GetAEspecificRating
{
    public record GetAEspecificRatingRequest(int UserId, int RecipeId); 

    public class GetAEspecificRatingEndpoint : Endpoint<GetAEspecificRatingRequest, GetAEspecificRatingResponse>
    {
        private readonly IMediator _mediator;

        public GetAEspecificRatingEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Get("/api/ratings/user/{userId}/recipe/{recipeId}");
            Summary(x =>
            {
                x.Summary = "Obtiene la calificación específica de un usuario para una receta.";
                x.Description = "Permite obtener la calificación que un usuario ha dado a una receta específica.";
            });
            Description(x => x.WithTags("Ratings"));
            AllowAnonymous(); 
        }

        public override async Task HandleAsync(GetAEspecificRatingRequest req, CancellationToken ct)
        {
            var query = new GetAEspecificRatingQuery(req.UserId, req.RecipeId);
            var result = await _mediator.Send(query);

            await Send.OkAsync(result); 
        }
        
    }
}
