using Microsoft.EntityFrameworkCore;
using User.API.Common.Database;

namespace User.API.repositories.UserRespository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _context;

        public UserRepository(UserDbContext context)
        {
            _context = context;
        }
        
        public async Task<Entities.User> AddUser(Entities.User entity)
        {
            _context.Users.Add(entity);
            await _context.SaveChangesAsync();

            return entity; 
        }

        public async Task DeleteUser(Entities.User entity)
        {
            _context.Users.Remove(entity);  
            await _context.SaveChangesAsync();
        }

        public async Task<Entities.User?> GetUser(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id); 
            return user; 
        }

        public async Task<Entities.User?> GetUserByEmail(string email)
        {
            var user = await _context.Users.Where(x => x.Email == email).FirstOrDefaultAsync();
            return user; 
        }

        public async Task<Entities.User> UpdateUser(Entities.User entity)
        {
            _context.Users.Update(entity); 
            await _context.SaveChangesAsync();

            // recargar la entidad para obtener los valores incluso recargados
            await _context.Entry(entity).ReloadAsync();
            return entity; 
        }
    }
}
