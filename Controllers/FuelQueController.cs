using Microsoft.AspNetCore.Mvc;
using equeue_server.Models;
using equeue_server.Services;

/*
* FuelQueController: class Implements ControllerBase: interface - Manages fuel queue routes and service mappings
*/
namespace equeue_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuelQueController : ControllerBase
    {
        private readonly IFuelQueService fuelQueService;

        public FuelQueController(IFuelQueService fuelQueService)
        {
            this.fuelQueService = fuelQueService;
        }

        // GET api/<UserController>
        // Handles - Get all fuel queues
        [HttpGet]
        public ActionResult<List<FuelQue>> Get()
        {
            return fuelQueService.Get();
        }

        // GET api/<UserController>/5
        // Handles - Get fuel queue for given fuel queue id
        [HttpGet("{id}")]
        public ActionResult<FuelQue> Get(String id)
        {
            var fuelQue = fuelQueService.Get(id);
            if (fuelQue == null)
            {
                return NotFound($"FuelQue with Id = {id} not found");
            }

            return fuelQue;
        }

        // GET api/<UserController>/5
        // Handles - Get fuel queue for given fuel station id
        [HttpGet("fuel-station/{id}")]
        public ActionResult<FuelQue> GetFuelQueByFuelStationId(String id)
        {
            var fuelQue = fuelQueService.GetFuelQueByFuelStationId(id);
            if (fuelQue == null)
            {
                return NotFound($"FuelQue with Station Id = {id} not found");
            }

            return fuelQue;
        }

        // POST api/<UserController>
        // Handles - Register fuel queue
        [HttpPost]
        public ActionResult<FuelQue> Post([FromBody] FuelQue fuelQue)
        {
            fuelQueService.Create(fuelQue);
            return CreatedAtAction(nameof(Get), new { id = fuelQue.Id }, fuelQue);
        }

        // POST api/<UserController>/5
        // Handles - Join customer to fuel queue
        [HttpPost("join/{id}")]
        public ActionResult<FuelQue> Join(string id, [FromBody] QueueCustomer queueCustomer)
        {
            Console.Write(queueCustomer);
            bool isUpdated = fuelQueService.AddUsersToQueue(queueCustomer, id);
            
            return CreatedAtAction(nameof(Get), new { status = isUpdated }, queueCustomer);
        }

        // PUT api/<UserController>/5
        // Handles - Leave customer from fuel queue
        [HttpPut("leave/{id}")]
        public ActionResult<FuelQue> Leave(string id, [FromBody] QueueCustomer queueCustomer)
        {
            bool isUpdated = fuelQueService.RemoveUsersFromQueue(id, queueCustomer.UserId, queueCustomer.DetailedStatus);

            return CreatedAtAction(nameof(Get), new { status = isUpdated }, queueCustomer);
        }

        // PUT api/<UserController>
        // Handles - Update fuel queue for gievn fuel queue id
        [HttpPut("{id}")]
        public ActionResult Put(String id, [FromBody] FuelQue fuelQue)
        {
            var existingFuelQue = fuelQueService.Get(id);
            if (existingFuelQue == null)
            {
                return NotFound($"FuelQue with Id = {id} not found");
            }

            fuelQueService.Update(id, fuelQue);

            return NoContent();
        }


        // DELETE api/<StudentController>/5
        // Handles - Delete fuel queue for given fuel queue id
        [HttpDelete("{id}")]
        public ActionResult Delete(String id)
        {
            var fuelQue = fuelQueService.Get(id);

            if (fuelQue == null)
            {
                return NotFound($"FuelQue with Id = {id} not found");
            }

            fuelQueService.Delete(fuelQue.Id);

            return Ok($"FuelQue with Id = {id} deleted");
        }
    }
}