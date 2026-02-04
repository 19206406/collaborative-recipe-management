using BuildingBlocks.CQRS;

namespace Recipe.API.Features.Recipe.GetRecipesByUser
{
    public record GetRecipesByUserQuery(int UserId) : IQuery<GetRecipesByUserResponse>;
}
