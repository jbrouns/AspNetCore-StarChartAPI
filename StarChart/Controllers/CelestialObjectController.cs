using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            this._context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var celestialObject = _context.CelestialObjects.FirstOrDefault(c => c.Id == id);

            if(celestialObject == null)
            {
                return NotFound();
            }

            var sattelites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == id).ToList();

            celestialObject.Satellites = sattelites;

            return Ok(celestialObject);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects = _context.CelestialObjects.Where(c => c.Name == name);

            if(!celestialObjects.Any())
            {
                return NotFound();
            }

            foreach(var celestialObject in celestialObjects)
            {
                var sattelites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == celestialObject.Id).ToList();
                celestialObject.Satellites = sattelites;
            }

             return Ok(celestialObjects.ToList());
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects;

            foreach(var celestialObject in celestialObjects)
            {
                var sattelites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == celestialObject.Id).ToList();
                celestialObject.Satellites = sattelites;
            }

            return Ok(celestialObjects.ToList());
        }

    }
}