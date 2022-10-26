using Microsoft.AspNetCore.Mvc;
using equeue_server.Models;
using equeue_server.Services;

/*
* UserController: class Implements ControllerBase: interface - Manages user routes and service mappings
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

        // GET api/<UserController>
        // Handles - Get all users
        [HttpGet]
        public ActionResult<List<User>> Get()
        {
            return userService.Get();
        }

        // GET api/<UserController>/5
        // Handles - Get user for given user id
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

        // POST api/<UserController>
        // Handles - Register user
        [HttpPost]
        public ActionResult<User> Post([FromBody] User user)
        {
            userService.Create(user);
            return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
        }

        // POST api/<UserController>
        // Handles - Login user
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

        // PUT api/<UserController>/5
        // Handles - Update user for gievn user id
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

        // DELETE api/<StudentController>/5
        // Handles - Delete user for given user id
        [HttpDelete("{id}")]
        public ActionResult Delete(String id)
        {
            var user = userService.Get(id);

            if (user == null)
            {
                return NotFound($"User with Id = {id} not found");
            }

            userService.Delete(user.Id);

            return Ok($"User with Id = {id} deleted");
        }
    }
}