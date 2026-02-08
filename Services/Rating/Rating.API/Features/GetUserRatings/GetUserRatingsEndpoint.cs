using FastEndpoints;
using MediatR;

namespace Rating.API.Features.GetUserRatings
{
    public record GetUserRatingsRequest(int UserId); 

    public class GetUserRatingsEndpoint : Endpoint<GetUserRatingsRequest, GetUserRatingsResponse>
    {
        private readonly IMediator _mediator;

        public GetUserRatingsEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Get("/api/ratings/user/{userId}");
            Summary(x =>
            {
                x.Summary = "Obtiene las calificaciones realizadas por un usuario específico.";
                x.Description = "Devuelve una lista de calificaciones realizadas por el usuario identificado por su ID.";
            });
            Description(x => x.WithTags("Ratings"));
            AllowAnonymous(); 
        }

        public override async Task HandleAsync(GetUserRatingsRequest req, CancellationToken ct)
        {
            var query = new GetUserRatingsQuery(req.UserId);
            var result = await _mediator.Send(query);

            await Send.OkAsync(result); 
        }
    }
}
