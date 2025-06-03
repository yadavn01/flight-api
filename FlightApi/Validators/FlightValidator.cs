using FluentValidation;
using Microsoft.EntityFrameworkCore;

public class FlightValidator : AbstractValidator<Flight>
{
    private readonly FlightContext _context;

    public FlightValidator(FlightContext context)
    {
        _context = context;

        RuleFor(f => f.DepartureTime)
            .Must((flight, departureTime) => departureTime < flight.ArrivalTime)
            .WithMessage("Departure time must be before arrival time");

        RuleFor(f => f)
            .MustAsync(async (flight, cancellation) => 
                !await IsDuplicateFlightNumber(flight))
            .WithMessage(f => 
                $"Flight number {f.FlightNumber} already exists for date {f.DepartureTime.Date:yyyy-MM-dd}");
    }

    private async Task<bool> IsDuplicateFlightNumber(Flight flight)
    {
        return await _context.Flights.AnyAsync(f =>
            f.Id != flight.Id && 
            f.FlightNumber == flight.FlightNumber && 
            f.DepartureTime.Date == flight.DepartureTime.Date);
    }
}