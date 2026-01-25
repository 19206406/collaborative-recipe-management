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
        
        public async Task<int> AddUser(Entities.User entity)
        {
            _context.Users.Add(entity);
            await _context.SaveChangesAsync();

            return entity.Id; 
        }

        public async Task DeleteUser(int id)
        {
            _context.Remove(id); 
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

        public async Task UpdateUser(Entities.User entity)
        {
            _context.Users.Update(entity); 
            await _context.SaveChangesAsync();
        }
    }
}
