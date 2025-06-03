using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;

public class FlightServiceTests
{
    private FlightService GetServiceWithInMemoryDb(string dbName)
    {
        var options = new DbContextOptionsBuilder<FlightContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
        var context = new FlightContext(options);
        var validator = new FlightValidator(context);
        var logger = new Mock<ILogger<FlightService>>().Object;
        return new FlightService(context, validator, logger);
    }

    [Fact]
    public async Task CreateFlightAsync_ValidFlight_AddsFlight()
    {
        var service = GetServiceWithInMemoryDb("CreateFlightDb");
        var flight = new Flight
        {
            FlightNumber = "QA123",
            Airline = "Qantas",
            DepartureAirport = "SYD",
            ArrivalAirport = "MEL",
            DepartureTime = DateTime.Now.AddHours(1),
            ArrivalTime = DateTime.Now.AddHours(3),
            Status = FlightStatus.Scheduled
        };

        var result = await service.CreateFlightAsync(flight);

        Assert.NotNull(result);
        Assert.Equal("QA123", result.FlightNumber);
    }

    [Fact]
    public async Task GetFlightByIdAsync_NonExistent_ThrowsNotFound()
    {
        var service = GetServiceWithInMemoryDb("GetFlightDb");
        await Assert.ThrowsAsync<FlightService.NotFoundException>(() => service.GetFlightByIdAsync(999));
    }
}