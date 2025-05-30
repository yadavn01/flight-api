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
    public string FlightNumber { get; set; }
    public string Airline { get; set; }
    public string DepartureAirport { get; set; }
    public string ArrivalAirport { get; set; }
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public FlightStatus Status { get; set; }
}