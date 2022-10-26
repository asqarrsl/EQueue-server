using Microsoft.AspNetCore.Mvc;
using equeue_server.Models;
using equeue_server.Services;

/*
* UserController: class Implements ControllerBase: interface - User routes and service mappings are managed 
*/
namespace MongoDBTestProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        // Get all users
        // GET api/<UserController>
        [HttpGet]
        public ActionResult<List<User>> Get()
        {
            return userService.Get();
        }

        // Get user by id
        // GET api/<UserController>/3
        [HttpGet("{id}")]
        public ActionResult<User> Get(String id)
        {
            var user = userService.Get(id);
            if (user == null)
            {
                return NotFound($"User with Id = {id} not found");
            }

            return user;
        }

        // User Registration 
        // POST api/<UserController>
        [HttpPost]
        public ActionResult<User> Post([FromBody] User user)
        {
            userService.Create(user);
            return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
        }

       
        // Update user by id
        // PUT api/<UserController>/3
        [HttpPut("{id}")]
        public ActionResult Put(String id, [FromBody] User user)
        {

            var existingUser = userService.Get(id);
            
            if (existingUser == null)
            {
                return NotFound($"User with Id = {id} not found");
            }

            userService.Update(id, user);

            return NoContent();
        }

        // Delete user by id
        // DELETE api/<StudentController>/3
        [HttpDelete("{id}")]
        public ActionResult Delete(String id)
        {
            var user = userService.Get(id);

            if (user == null)
            {
                return NotFound($"User with Id, {id} not found");
            }

            userService.Delete(user.Id);

            return Ok($"User with Id, {id} deleted");
        }

         // User Login
        // POST api/<UserController>
        [HttpPost("login")]
        public ActionResult<User> Login([FromBody] User request)
        {
            if (request.Username == null || request.Password == null || request.Role == null)
            {
                return BadRequest("Please provide username and password");
            }

            var existingUser = userService.Login(request.Username, request.Password, request.Role);

            if (existingUser == null)
            {
                return NotFound($"User with username = {request.Username} not found");
            }
           

            return Ok(existingUser);
        }
    }
    
}