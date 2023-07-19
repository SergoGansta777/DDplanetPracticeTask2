using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice_task2.Model;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

[ApiController]
[Route("api/addressinlocations")]
public class AddressInLocationController : ControllerBase
{
    private readonly PracticeTask2Context _dbContext;

    public AddressInLocationController(PracticeTask2Context dbContext)
    {
        _dbContext = dbContext;
    }

    // GET: api/addressinlocations
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AddressInLocation>>> GetAddressInLocations()
    {
        try
        {
            var addressInLocations = await _dbContext.AddressInLocations.ToListAsync();
            return Ok(addressInLocations);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while retrieving address in locations");
        }
    }

    // GET: api/addressinlocations/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<AddressInLocation>> GetAddressInLocation(int id)
    {
        try
        {
            var addressInLocation = await _dbContext.AddressInLocations.FindAsync(id);
            if (addressInLocation == null)
            {
                return NotFound("Address in location by id " + id + " not found");
            }
            return Ok(addressInLocation);
        }
        catch (Exception)
        {
            return StatusCode(
                500,
                "An error occurred while retrieving address in location by id " + id
            );
        }
    }

    [HttpPost]
    public async Task<ActionResult<AddressInLocation>> AddAddressToLocation(
        AddressInLocation addressInLocation
    )
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            _dbContext.AddressInLocations.Add(addressInLocation);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetAddressInLocation),
                new { id = addressInLocation.LocationId },
                addressInLocation
            );
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while adding address to location");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveAddressFromLocation(int id)
    {
        var addressInLocation = await _dbContext.AddressInLocations.FindAsync(id);
        if (addressInLocation == null)
        {
            return NotFound("AddressInLocation by id " + id + " not found");
        }

        try
        {
            _dbContext.AddressInLocations.Remove(addressInLocation);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while removing address from location");
        }
        return NoContent();
    }

    private async Task<bool> AddressInLocationExists(int id)
    {
        return await _dbContext.AddressInLocations.AnyAsync(e => e.LocationId == id);
    }
}
