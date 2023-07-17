// Add the necessary using statements
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice_task2.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Define the controller for the API
[ApiController]
[Route("api/locations")]
public class ApiController : ControllerBase
{
    private readonly PracticeTask2Context _Dbcontext;

    public ApiController(PracticeTask2Context context)
    {
        _Dbcontext = context;
    }

    // GET: api/locations
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Location>>> GetLocations()
    {
        return await _Dbcontext.Locations.ToListAsync();
    }

    // GET: api/locations/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Location>> GetLocation(int id)
    {
        var location = await _Dbcontext.Locations.FindAsync(id);

        if (location == null)
        {
            return NotFound();
        }

        return location;
    }

    // POST: api/locations
    [HttpPost]
    public async Task<ActionResult<Location>> CreateLocation(Location location)
    {
        _Dbcontext.Locations.Add(location);
        await _Dbcontext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetLocation), new { id = location.Id }, location);
    }

    // PUT: api/locations/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLocation(int id, Location location)
    {
        if (id != location.Id)
        {
            return BadRequest();
        }

        _Dbcontext.Entry(location).State = EntityState.Modified;

        try
        {
            await _Dbcontext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!LocationExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/locations/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLocation(int id)
    {
        var location = await _Dbcontext.Locations.FindAsync(id);
        if (location == null)
        {
            return NotFound();
        }

        _Dbcontext.Locations.Remove(location);
        await _Dbcontext.SaveChangesAsync();

        return NoContent();
    }

    // POST: api/locations/{id}/addresses
    [HttpPost("{id}/addresses")]
    public async Task<IActionResult> AddAddressToLocation(int id, AddressInLocation address)
    {
        return Ok();
    }

    // DELETE: api/locations/{locationId}/addresses/{addressId}
    [HttpDelete("{locationId}/addresses/{addressId}")]
    public async Task<IActionResult> RemoveAddressFromLocation(int locationId, int addressId)
    {
        return Ok();
    }

    private bool LocationExists(int id)
    {
        return _Dbcontext.Locations.Any(e => e.Id == id);
    }
}