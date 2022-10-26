/*
* IUserInterface: interface - Interface for managing user operations on database
*/

using equeue_server.Models;

namespace equeue_server.Services
{
    public interface IUserService
    {
        List<User> Get();
        User Get(string id);
        User Login(string username, string password, string role);
        User Create(User user);
        void Update(string id, User user);
        void Delete(string id);
    }
}

