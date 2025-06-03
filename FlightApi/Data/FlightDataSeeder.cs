public static class FlightDataSeeder
{
    public static void Seed(FlightContext context)
    {
        if (!context.Flights.Any()) //avoid duplicates
        {
            var lines = File.ReadAllLines("Data/FlightInformation.csv");

            foreach (var line in lines.Skip(1)) 
            {
                var values = line.Split(',');
                var flight = new Flight
                {
                    Id = int.Parse(values[0]), 
                    FlightNumber = values[1],
                    Airline = values[2],
                    DepartureAirport = values[3],
                    ArrivalAirport = values[4],
                    DepartureTime = DateTime.Parse(values[5]),
                    ArrivalTime = DateTime.Parse(values[6]),
                    Status = Enum.Parse<FlightStatus>(values[7])
                };
                context.Flights.Add(flight);
            }
            context.SaveChanges();
        }
    }
}