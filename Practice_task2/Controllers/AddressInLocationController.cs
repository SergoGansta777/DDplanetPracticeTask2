using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice_task2.Model;

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

    // POST: api/addressinlocation
    [HttpPost]
    public async Task<ActionResult<AddressInLocation>> AddAddressToLocation(
        AddressInLocation addressInLocation
    )
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        if (!IsAddressInLocationComplete(addressInLocation))
        {
            return BadRequest("Address in location is not complete");
        }

        bool isLocationOverlapping = await _dbContext.AddressInLocations
            .Join(
                _dbContext.Locations,
                ail => ail.LocationId,
                l => l.Id,
                (ail, l) => new { AddressInLocation = ail, Location = l }
            )
            .AnyAsync(
                x =>
                    x.AddressInLocation.FiasRegionCode == addressInLocation.FiasRegionCode
                    && (
                        x.AddressInLocation.FiasHouseCode == addressInLocation.FiasHouseCode
                        || x.AddressInLocation.FiasStreetCode == addressInLocation.FiasStreetCode
                        || x.AddressInLocation.FiasCityCode == addressInLocation.FiasCityCode
                    )
            );

        if (isLocationOverlapping)
        {
            return BadRequest("Address in location for current managing company is overlapping");
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

    // DELETE: api/addressinlocation/{id}
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

    private bool IsAddressInLocationComplete(AddressInLocation addressInLocation)
    {
        var isFiasHouseCodeExists = !string.IsNullOrWhiteSpace(addressInLocation.FiasHouseCode);
        var isFiasStreetCodeExists = !string.IsNullOrWhiteSpace(addressInLocation.FiasStreetCode);
        var isFiasCityCodeExists = !string.IsNullOrWhiteSpace(addressInLocation.FiasCityCode);
        var isFiasRegionCodeExists = !string.IsNullOrWhiteSpace(addressInLocation.FiasRegionCode);

        var isFiasStreetCodeMissedButRequired =
            isFiasHouseCodeExists && !isFiasStreetCodeExists && isFiasCityCodeExists;
        var isFiasCityCodeMissedButRequired =
            isFiasStreetCodeExists && !isFiasCityCodeExists && isFiasRegionCodeExists;
        var isFiasRegionCodeMissedButRequired = isFiasCityCodeExists && !isFiasRegionCodeExists;

        var isSomethingMissed =
            isFiasStreetCodeMissedButRequired
            || isFiasCityCodeMissedButRequired
            || isFiasRegionCodeMissedButRequired;

        return !isSomethingMissed;
    }

    protected async Task<bool> AddressInLocationExists(int id)
    {
        return await _dbContext.AddressInLocations.AnyAsync(e => e.LocationId == id);
    }
}
