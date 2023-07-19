using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice_task2.Model;
using System.Text.RegularExpressions;

[ApiController]
[Route("api/locations")]
public class LocationController : ControllerBase
{
    private readonly PracticeTask2Context _dbContext;

    public LocationController(PracticeTask2Context context)
    {
        _dbContext = context;
    }

    // GET: api/locations
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Location>>> GetLocations()
    {
        try
        {
            var locations = await _dbContext.Locations.ToListAsync();
            return Ok(locations);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while retrieving locations");
        }
    }

    // GET: api/locations/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Location>> GetLocation(int id)
    {
        try
        {
            var location = await _dbContext.Locations.FindAsync(id);
            if (location == null)
            {
                return NotFound("Location by id " + id + " not found");
            }
            return Ok(location);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while retrieving location by id " + id);
        }
    }

    // POST: api/locations
    [HttpPost]
    public async Task<ActionResult<Location>> CreateLocation(Location location)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        // Check if a location with the same managing company already exists
        // var existingLocation = await _dbContext.Locations
        //     .FirstOrDefaultAsync(l => l.ManagingCompany == location.ManagingCompany);
        // if (existingLocation != null)
        // {
        //     return BadRequest("Location with the same managing company already exists");
        // }

        try
        {
            _dbContext.Locations.Add(location);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLocation), new { id = location.Id }, location);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while creating location");
        }
    }

    // PUT: api/locations/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLocation(int id, Location location)
    {
        if (id != location.Id)
        {
            return BadRequest();
        }
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        if (!await LocationExists(id))
        {
            return NotFound("Location by id " + id + " not found");
        }

        // Check if a location with the same managing company exists
        // var existingLocation = await _dbContext.Locations
        //     .FirstOrDefaultAsync(l => l.ManagingCompany == location.ManagingCompany && l.Id != location.Id);
        // if (existingLocation != null)
        // {
        //     return BadRequest("Location with the same managing company already exists");
        // }

        try
        {
            _dbContext.Entry(location).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while updating location");
        }
        return NoContent();
    }

    // DELETE: api/locations/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLocation(int id)
    {
        var location = await _dbContext.Locations.FindAsync(id);
        if (location == null)
        {
            return NotFound("Location by id " + id + " not found");
        }

        try
        {
            _dbContext.Locations.Remove(location);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while deleting location");
        }
        return NoContent();
    }

    private async Task<bool> LocationExists(int id)
    {
        return await _dbContext.Locations.AnyAsync(e => e.Id == id);
    }
}
