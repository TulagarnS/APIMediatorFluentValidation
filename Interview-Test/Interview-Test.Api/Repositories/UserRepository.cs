using System.Runtime.InteropServices.Marshalling;
using Interview_Test.Infrastructure;
using Interview_Test.Models;
using Interview_Test.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Interview_Test.Repositories;

public class UserRepository : IUserRepository
{

    private readonly InterviewTestDbContext _context;

    public UserRepository(InterviewTestDbContext context) //DbContext Dependency injection
    {
        _context = context;
    }
    public dynamic GetUserById(string id)
    {
        Guid guidId = Guid.Parse(id);
        UserModel? user = _context.UserTb
            .Include(include => include.UserProfile)
            .Include(include => include.UserRoleMappings)
            .ThenInclude(thenInclude => thenInclude.Role)
            .ThenInclude(thenInclude => thenInclude.Permissions) 
            .FirstOrDefault(u => u.Id == guidId);
        return user;
    }

    public int CreateUser(UserModel user)
    {
        try
        {
            int result;
            _context.UserTb.Add(user);
            result = _context.SaveChanges();
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public bool CheckUserIfExistsByUserId(string id)
    {
        return _context.UserTb.Any(condition => condition.Id.ToString() == id);
    }
}