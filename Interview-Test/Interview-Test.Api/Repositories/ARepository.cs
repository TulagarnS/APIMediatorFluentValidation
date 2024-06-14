using Interview_Test.Models;
using Interview_Test.Repositories.Interfaces;

namespace Interview_Test.Repositories;

public class ARepository : IUserRepository
{
    public dynamic GetUserById(string id)
    {
        throw new NotImplementedException();
    }

    public int CreateUser(UserModel user)
    {
        throw new NotImplementedException();
    }

    public bool CheckUserIfExistsByUserId(string id)
    {
        throw new NotImplementedException();
    }
}