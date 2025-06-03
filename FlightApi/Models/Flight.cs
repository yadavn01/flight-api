using System.ComponentModel.DataAnnotations;

public enum FlightStatus
{
    Scheduled,
    Delayed,
    Cancelled,
    InAir,
    Landed
}

public class Flight
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Flight number is required")]
    [StringLength(5, MinimumLength = 5, ErrorMessage = "Flight number must be 5 characters")]
    public string FlightNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Airline name is required")]
    public string Airline { get; set; } = string.Empty;

    [Required(ErrorMessage = "Departure airport is required")]
    [StringLength(3, MinimumLength = 3, ErrorMessage = "Airport code must be 3 characters")]
    public string DepartureAirport { get; set; } = string.Empty;

    [Required(ErrorMessage = "Arrival airport is required")]
    [StringLength(3, MinimumLength = 3, ErrorMessage = "Airport code must be 3 characters")]
    public string ArrivalAirport { get; set; } = string.Empty;

    [Required(ErrorMessage = "Departure time is required")]
    public DateTime DepartureTime { get; set; }

    [Required(ErrorMessage = "Arrival time is required")]
    public DateTime ArrivalTime { get; set; }

    [Required(ErrorMessage = "Flight status is required")]
    public FlightStatus Status { get; set; }
}