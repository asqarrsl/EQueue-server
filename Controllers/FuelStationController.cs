using equeue_server.Models;
using equeue_server.Services;
using Microsoft.AspNetCore.Mvc;

/*
* FuelStationController: class Implements ControllerBase: interface - Manages fuel station routes and service mappings
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

        // GET: api/<FuelStationController>
        // Handles - Get all fuel stations
        [HttpGet]
        public ActionResult<List<FuelStation>> Get()
        {
            return fuelStationService.Get();
        }

        // GET api/<FuelStationController>/5
        // Handles - Get fuel station for given fuel station id
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

        // POST api/<FuelStationController>
        // Handles - Register fuel station
        [HttpPost]
        public ActionResult<FuelStation> Post([FromBody] FuelStation fuelStation)
        {
            fuelStationService.Create(fuelStation);
            return CreatedAtAction(nameof(Get), new { id = fuelStation.Id }, fuelStation);
        }

        // PUT api/<FuelStationController>/5
        // Handles - Update fuel station for gievn fuel station id
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

        // DELETE api/<FuelStationController>/5
        // Handles - Delete fuel station for given fuel station id
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
