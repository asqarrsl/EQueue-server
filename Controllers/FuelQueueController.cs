using Microsoft.AspNetCore.Mvc;
using equeue_server.Models;
using equeue_server.Services;

/*
* FuelQueueController: class Implements ControllerBase: interface - Manages fuel queue routes and service mappings
*/
namespace equeue_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuelQueueController : ControllerBase
    {
        private readonly IFuelQueueService fuelQueueService;

        public FuelQueueController(IFuelQueueService fuelQueueService)
        {
            this.fuelQueueService = fuelQueueService;
        }

        // GET api/<UserController>
        // Handles - Get all fuel queues
        [HttpGet]
        public ActionResult<List<FuelQueue>> Get()
        {
            return fuelQueueService.Get();
        }

        // GET api/<UserController>/5
        // Handles - Get fuel queue for given fuel queue id
        [HttpGet("{id}")]
        public ActionResult<FuelQueue> Get(String id)
        {
            var fuelQueue = fuelQueueService.Get(id);
            if (fuelQueue == null)
            {
                return NotFound($"FuelQueue with Id = {id} not found");
            }

            return fuelQueue;
        }

        // POST api/<UserController>
        // Handles - Register fuel queue
        [HttpPost]
        public ActionResult<FuelQueue> Post([FromBody] FuelQueue fuelQueue)
        {
            fuelQueueService.Create(fuelQueue);
            return CreatedAtAction(nameof(Get), new { id = fuelQueue.Id }, fuelQueue);
        }

        // POST api/<UserController>/5
        // Handles - Join customer to fuel queue
        [HttpPost("/join/{id}")]
        public ActionResult<FuelQueue> Post(string id, [FromBody] QueueCustomer queueCustomer)
        {
            bool isUpdated = fuelQueueService.AddUsersToQueue(queueCustomer, id);

            return CreatedAtAction(nameof(Get), new { status = isUpdated }, queueCustomer);
        }

        // PUT api/<UserController>/5
        // Handles - Leave customer from fuel queue
        [HttpPut("/leave/{id}")]
        public ActionResult<FuelQueue> Put(string id, [FromBody] QueueCustomer queueCustomer)
        {
            bool isUpdated = fuelQueueService.RemoveUsersFromQueue(id, queueCustomer.UserId, queueCustomer.DetailedStatus);

            return CreatedAtAction(nameof(Get), new { status = isUpdated }, queueCustomer);
        }

        // PUT api/<UserController>
        // Handles - Update fuel queue for gievn fuel queue id
        [HttpPut("{id}")]
        public ActionResult Put(String id, [FromBody] FuelQueue fuelQueue)
        {
            var existingFuelQueue = fuelQueueService.Get(id);

            if (existingFuelQueue == null)
            {
                return NotFound($"FuelQueue with Id = {id} not found");
            }

            fuelQueueService.Update(id, fuelQueue);

            return NoContent();
        }

        // DELETE api/<StudentController>/5
        // Handles - Delete fuel queue for given fuel queue id
        [HttpDelete("{id}")]
        public ActionResult Delete(String id)
        {
            var fuelQueue = fuelQueueService.Get(id);

            if (fuelQueue == null)
            {
                return NotFound($"FuelQueue with Id = {id} not found");
            }

            fuelQueueService.Delete(fuelQueue.Id);

            return Ok($"FuelQueue with Id = {id} deleted");
        }
    }
}
