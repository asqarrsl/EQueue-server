using equeue_server.Models;
using equeue_server.Services;
using Microsoft.AspNetCore.Mvc;

/*
* FuelStationController: class Implements ControllerBase: interface - fuel station routes and service mappings are managed 
*/
namespace equeue_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuelStationController : ControllerBase
    {
        private readonly IFuelStationService fuelStationService;

        public FuelStationController(IFuelStationService fuelStationService)
        {
            this.fuelStationService = fuelStationService;
        }

        // Get all fuel stations
        // GET: api/<FuelStationController>
        [HttpGet]
        public ActionResult<List<FuelStation>> Get()
        {
            return fuelStationService.Get();
        }

        // Get fuel station by id
        // GET api/<FuelStationController>/3
        [HttpGet("{id}")]
        public ActionResult<FuelStation> Get(String id)
        {
            var fuelStation = fuelStationService.Get(id);
            if (fuelStation == null)
            {
                return NotFound($"FuelStation with Id = {id} not found");
            }

            return fuelStation;
        }

        // Register fuel station
        // POST api/<FuelStationController>
        [HttpPost]
        public ActionResult<FuelStation> Post([FromBody] FuelStation fuelStation)
        {
            fuelStationService.Create(fuelStation);
            return CreatedAtAction(nameof(Get), new { id = fuelStation.Id }, fuelStation);
        }

        // Update fuel station by id
        // PUT api/<FuelStationController>/3
        [HttpPut("{id}")]
        public ActionResult Put(String id, [FromBody] FuelStation fuelStation)
        {
            var existingStation= fuelStationService.Get(id);

            if (existingStation == null)
            {
                return NotFound($"FuelStationService with Id = {id} not found");
            }

            fuelStationService.Update(id, fuelStation);

            return NoContent();
        }

        // Delete fuel station by id
        // DELETE api/<FuelStationController>/3
        [HttpDelete("{id}")]
        public ActionResult Delete(String id)
        {
            var fuelStation = fuelStationService.Get(id);

            if (fuelStation == null)
            {
                return NotFound($"FuelStation with Id = {id} not found");
            }

            fuelStationService.Delete(fuelStation.Id);

            return Ok($"FuelStation with Id = {id} deleted");
        }
    }
}
