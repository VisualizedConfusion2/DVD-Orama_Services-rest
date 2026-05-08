using DVD_Orama_Services_rest.Models.Entities;
namespace DVD_Orama_Services_rest.Repos.Interfaces
{
    public interface IUserRepo
    {
        public IEnumerable<User> GetAll();
        public User GetById(int id);

    }
}
