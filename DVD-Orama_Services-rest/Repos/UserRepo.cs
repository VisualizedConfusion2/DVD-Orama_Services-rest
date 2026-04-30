using DVD_Orama_Services_rest.Data;
using DVD_Orama_Services_rest.Models.Entities;
using DVD_Orama_Services_rest.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DVD_Orama_Services_rest.Repos
{
    public class UserRepo : IUserRepo
    {
        private readonly AppDbContext _dbContext;

        public UserRepo() 
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(Secrets.ConnectionString);
            _dbContext = new AppDbContext(optionsBuilder.Options);
        }

        public IEnumerable<User> GetAll()
        {
            return _dbContext.Users;
        }

        public User? GetById(int id)
        {
            return _dbContext.Users.FirstOrDefault(user => user.Id == id);
        }
    }
}
