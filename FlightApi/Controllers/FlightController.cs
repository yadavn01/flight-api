using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static FlightService;


[Route("api/flights")]
[ApiController]
public class FlightController(FlightContext context, IFlightService flightService) : ControllerBase
{
    private readonly FlightContext _context = context;
    private readonly IFlightService _flightService = flightService;

    [HttpGet]
    public IActionResult GetFlights()
    {
        try
        {
            var flights = _context.Flights.ToList();
            return Ok(flights);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while retrieving flights");
        }
    }

    [HttpGet("{id}")]
    public IActionResult GetFlight(int id)
    {
        try
        {
            var flight = _context.Flights.Find(id);
            return Ok(flight);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while retrieving the flight");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateFlight([FromBody] Flight flight)
    {
        try
        {
            var newFlight = await _flightService.CreateFlightAsync(flight);
            return CreatedAtAction(nameof(GetFlight), new { id = newFlight.Id }, newFlight);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while creating the flight");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFlight(int id, [FromBody] Flight flight)
    {
        try
        {
            var updatedFlight = await _flightService.UpdateFlightAsync(id, flight);
            return Ok(updatedFlight);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while updating the flight");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFlight(int id)
    {
        try
        {
            await _flightService.DeleteFlightAsync(id);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while deleting the flight");
        }
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchFlights(
        [FromQuery] string? airline,
        [FromQuery] string? departureAirport,
        [FromQuery] string? arrivalAirport,
        [FromQuery] DateTime? departureFrom,
        [FromQuery] DateTime? departureTo,
        [FromQuery] DateTime? arrivalFrom,
        [FromQuery] DateTime? arrivalTo,
        [FromQuery] FlightStatus? status)
    {
        try
        {
            var results = await _flightService.SearchFlightsAsync(
                airline, departureAirport, arrivalAirport,
                departureFrom, departureTo, arrivalFrom, arrivalTo, status);
            return Ok(results);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while searching for flights");
        }
    }
}