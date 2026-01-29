using Microsoft.EntityFrameworkCore;
using Recipe.API.Entities;

namespace Recipe.API.Common.Database
{
    public class RecipeDbContext : DbContext
    {
        public RecipeDbContext(DbContextOptions<RecipeDbContext> options) : base(options)
        {
        }

        public DbSet<Entities.Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<RecipeTag> RecipeTags { get; set; }
        public DbSet<Step> Steps { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RecipeDbContext).Assembly); 
            base.OnModelCreating(modelBuilder);
        }

        //POST   /api/recipes                    - Crear receta(Auth)
        //GET    /api/recipes                    - Listar con paginación y filtros
        //GET    /api/recipes/{id}               - Detalle de receta
        //PUT    /api/recipes/{id}               - Actualizar(Auth, solo dueño)
        //DELETE /api/recipes/{id}               - Eliminar(Auth, solo dueño)
        //GET    /api/recipes/user/{userId}      - Recetas de un usuario
        //GET    /api/recipes/search             - Búsqueda avanzada
        //PUT    /api/recipes/{id}/rating        - Actualizar rating(interno)
        //GET    /api/recipes/by-ingredients     - Para Recommendation Service
    }
}
