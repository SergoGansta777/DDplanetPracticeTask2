// Add the necessary using statements
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice_task2.Model;
using System.Text.RegularExpressions;

[ApiController]
[Route("api/locations")]
public class CatalogController : ControllerBase
{
    private readonly PracticeTask2Context _Dbcontext;

    public CatalogController(PracticeTask2Context context)
    {
        _Dbcontext = context;
    }

    // GET: api/locations
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Location>>> GetLocations()
    {
        try
        {
            var locations = await _Dbcontext.Locations.ToListAsync();
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
            var location = await _Dbcontext.Locations.FindAsync(id);
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

        try
        {
            _Dbcontext.Locations.Add(location);
            await _Dbcontext.SaveChangesAsync();

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

        try
        {
            _Dbcontext.Entry(location).State = EntityState.Modified;
            await _Dbcontext.SaveChangesAsync();
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
        var location = await _Dbcontext.Locations.FindAsync(id);
        if (location == null)
        {
            return NotFound("Location by id " + id + " not found");
        }

        try
        {
            _Dbcontext.Locations.Remove(location);
            await _Dbcontext.SaveChangesAsync();
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while deleting location");
        }
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

    private async Task<bool> LocationExists(int id)
    {
        return await _Dbcontext.Locations.AnyAsync(e => e.Id == id);
    }
}
