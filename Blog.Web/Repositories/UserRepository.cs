using Blog.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthDbContext _db;

        public UserRepository(AuthDbContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<IdentityUser>> GetAll()
        {
            var users = await _db.Users.ToListAsync();
            var superAdmin = await _db.Users.FirstOrDefaultAsync(x => x.Email == "superadmin@blog.com");

            if (superAdmin != null)
            {
                users.Remove(superAdmin);
            }

            return users;
        }
    }
}
