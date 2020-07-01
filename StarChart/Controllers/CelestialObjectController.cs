using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

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

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject celestialObject)
        {
            _context.Add(celestialObject);
            _context.SaveChanges();

            return CreatedAtRoute("GetById", new { id = celestialObject.Id }, celestialObject);
        }

       [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestialObject)
        {
            var c = _context.CelestialObjects.FirstOrDefault(ce => ce.Id == id);

            if(c == null)
            {
                return NotFound();
            }

            c.Name = celestialObject.Name;
            c.OrbitalPeriod = celestialObject.OrbitalPeriod;
            c.OrbitedObjectId = celestialObject.OrbitedObjectId;

            _context.Update(c);
            _context.SaveChanges();

            return NoContent();
        }

       [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var c = _context.CelestialObjects.FirstOrDefault(ce => ce.Id == id);

            if(c == null)
            {
                return NotFound();
            }

            c.Name = name;
 
            _context.Update(c);
            _context.SaveChanges();

            return NoContent();
        }

       [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var list = _context.CelestialObjects.Where(ce => ce.Id == id || ce.OrbitedObjectId == id);

            if(!list.Any())
            {
                return NotFound();
            }

            _context.RemoveRange(list);
            _context.SaveChanges();

           return NoContent();
        }

    }
}