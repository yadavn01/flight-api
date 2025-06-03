using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public interface IFlightService
{
    Task<IEnumerable<Flight>> GetAllFlightsAsync();
    Task<Flight> GetFlightByIdAsync(int id);
    Task<Flight> CreateFlightAsync(Flight flight);
    Task<Flight> UpdateFlightAsync(int id, Flight flight);
    Task DeleteFlightAsync(int id);

    Task<IEnumerable<Flight>> SearchFlightsAsync(
        string? airline,
        string? departureAirport,
        string? arrivalAirport,
        DateTime? departureFrom,
        DateTime? departureTo,
        DateTime? arrivalFrom,
        DateTime? arrivalTo,
        FlightStatus? status
    );
}

public class FlightService : IFlightService
{
    private readonly FlightContext _context;
    private readonly FlightValidator _validator;
    private readonly ILogger<FlightService> _logger;

    public FlightService(FlightContext context, FlightValidator validator, ILogger<FlightService> logger)
    {
        _context = context;
        _validator = validator;
        _logger = logger;
    }

    public async Task<IEnumerable<Flight>> GetAllFlightsAsync()
    {
        return await _context.Flights.ToListAsync();
    }

    public async Task<Flight> GetFlightByIdAsync(int id)
    {
        var flight = await _context.Flights.FindAsync(id);
        if (flight == null)
        {
            throw new NotFoundException($"Flight with ID {id} not found");
        }
        return flight;
    }

    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }

    public async Task<Flight> CreateFlightAsync(Flight flight)
    {
        _logger.LogInformation("Creating new flight with number: {FlightNumber}", flight.FlightNumber);
        var validationResult = await _validator.ValidateAsync(flight);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));
        }

        _context.Flights.Add(flight);
        await _context.SaveChangesAsync();
        return flight;
    }

    public async Task<Flight> UpdateFlightAsync(int id, Flight flight)
    {
        _logger.LogInformation("Updating flight with ID: {FlightId}", id);
        if (id != flight.Id)
        {
            throw new ArgumentException("ID mismatch");
        }

        var existingFlight = await _context.Flights.FindAsync(id);
        if (existingFlight == null)
        {
            _logger.LogWarning("Flight with ID {FlightId} not found", id);
            throw new NotFoundException($"Flight with ID {id} not found");
        }

        var validationResult = await _validator.ValidateAsync(flight);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));
        }

        _context.Entry(existingFlight).CurrentValues.SetValues(flight);
        await _context.SaveChangesAsync();
        return flight;
    }

    public async Task DeleteFlightAsync(int id)
    {
        _logger.LogInformation("Deleting flight with ID: {FlightId}", id);
        var flight = await _context.Flights.FindAsync(id)
            ?? throw new NotFoundException($"Flight with ID {id} not found");

        _context.Flights.Remove(flight);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Flight>> SearchFlightsAsync(
        string? airline,
        string? departureAirport,
        string? arrivalAirport,
        DateTime? departureFrom,
        DateTime? departureTo,
        DateTime? arrivalFrom,
        DateTime? arrivalTo,
        FlightStatus? status)
    {
        _logger.LogInformation("Searching flights with filters: Airline={Airline}, DepartureAirport={DepartureAirport}, ArrivalAirport={ArrivalAirport}, DepartureFrom={DepartureFrom}, DepartureTo={DepartureTo}, ArrivalFrom={ArrivalFrom}, ArrivalTo={ArrivalTo}, Status={Status}",
        airline, departureAirport, arrivalAirport, departureFrom, departureTo, arrivalFrom, arrivalTo, status);
        
        var query = _context.Flights.AsQueryable();

        if (!string.IsNullOrWhiteSpace(airline))
            query = query.Where(f => f.Airline == airline);

        if (!string.IsNullOrWhiteSpace(departureAirport))
            query = query.Where(f => f.DepartureAirport == departureAirport);

        if (!string.IsNullOrWhiteSpace(arrivalAirport))
            query = query.Where(f => f.ArrivalAirport == arrivalAirport);

        if (departureFrom.HasValue)
            query = query.Where(f => f.DepartureTime >= departureFrom.Value);

        if (departureTo.HasValue)
            query = query.Where(f => f.DepartureTime <= departureTo.Value);

        if (arrivalFrom.HasValue)
            query = query.Where(f => f.ArrivalTime >= arrivalFrom.Value);

        if (arrivalTo.HasValue)
            query = query.Where(f => f.ArrivalTime <= arrivalTo.Value);

        if (status.HasValue)
            query = query.Where(f => f.Status == status.Value);

        return await query.ToListAsync();
    }
}