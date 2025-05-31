using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;


[Route("api/flights")]
[ApiController]
public class FlightController : ControllerBase
{
    private readonly FlightContext _context;

    public FlightController(FlightContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetFlights()
    {
        var flights = _context.Flights.ToList();
        return Ok(flights);
    }

    [HttpGet("{id}")]
    public IActionResult GetFlight(int id)
    {
        var flight = _context.Flights.Find(id);
        if (flight == null)
        {
            return NotFound();
        }
        return Ok(flight);
    }


}