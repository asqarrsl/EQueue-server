/*
* UserService: class Implements IUserInterface: interface - user operations on database are managed
*/

using MongoDB.Driver;
using equeue_server.Models;
using equeue_server.Models.Database;


namespace equeue_server.Services
{
    public class UserService : IUserService
    {
        // variable to hold mongodb collection
        private readonly IMongoCollection<User> _user;

        // constructor - retrives collections and assign collection to _user
        public UserService(IStoreDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _user = database.GetCollection<User>(settings.UserCollectionName);
        }

        /*
        * Function - Registering users
        * Params - user(User) - User object to register
        * Returns - registered user(User)
        */
        public User Create(User user)
        {
            _user.InsertOne(user);
            return user;
        }

        /*
        * Function - Deleting users
        * Params - id(string) - user id to remove
        * Returns - void
        */
        public void Delete(string id)
        {
            _user.DeleteOne(user => user.Id == id);
        }

        /*
        * Function - Retrieving users
        * Params - no params
        * Returns - List<User> list of user objects
        */
        public List<User> Get()
        {
            return _user.Find(user => true).ToList();
        }

        /*
        * Function - Retrieving user
        * Params - id(string) - user id to retrive
        * Returns - User user object associated with id
        */
        public User Get(string id)
        {
            return _user.Find(user => user.Id == id).FirstOrDefault();
        }

        /*
        * Function - Updating user
        * Params - id(string) - user id to retrive
        * Returns - User user object associated with id
        */
        public void Update(string id, User user)
        {
            _user.ReplaceOne(user => user.Id == id, user);
        }

        /*
        * Function - Login user
        * Params - username(string) - username
        *        - password(string) - password
        *        - role(string) - user role (customer / station-owner)
        * Returns - User user object associated with username, password, role
        */
        public User Login(string username, string password, string role)
        {
            return _user.Find(user => user.Username == username && user.Password == password && user.Role == role).FirstOrDefault();
        }
    }
}

