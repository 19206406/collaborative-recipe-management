using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
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

        //POST   /api/recipes                    - Crear receta(Auth) ---------- Listo con autenticación 
        //GET    /api/recipes                    - Listar con paginación y filtros ------------ Listo
        //GET    /api/recipes/{id}               - Detalle de receta --------- listo 
        //PUT    /api/recipes/{id}               - Actualizar(Auth, solo dueño) ------ listo ------- listo con autenticación 
        //DELETE /api/recipes/{id}               - Eliminar(Auth, solo dueño) ------- listo --------- listo con autenticación 
        //GET    /api/recipes/user/{userId}      - Recetas de un usuario ------- Listo
        //GET    /api/recipes/search             - Búsqueda avanzada ------- listo
        //PUT    /api/recipes/{id}/rating        - Actualizar rating(interno) ---- listo 
        //GET    /api/recipes/by-ingredients     - Para Recommendation Service ------ no la entiendo 

        //1. **Crear Receta**:
        //- Validar userId del JWT
        //- Validar mínimo 3 ingredientes y 3 pasos
        //- Normalizar ingredientes a minúsculas
        //- Auto-generar tags según ingredientes(si tiene "pollo" → tag "chicken")
        //- Calcular total_time = prep_time + cook_time

        //-Recibir nuevo promedio y contador desde Rating Service
        //-Actualizar campos average_rating y rating_count

//        Add-Migration NombreMigracion -Project Persistence -StartupProject Api -OutputDir Migrations
//Update-Database -Project Persistence -StartupProject Api

    }
}
