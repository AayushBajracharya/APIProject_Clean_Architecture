using APIProject.Models;

namespace APIProject.Repo
{
    public interface IUserRepository
    {
        User Authenticate(string username, string password);
        User GetById(int id);
    }

}

