using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//EF Core with an in-memory database
builder.Services.AddDbContext<FlightContext>(options =>
    options.UseInMemoryDatabase("FlightDb"));

builder.Services.AddControllers();
var app = builder.Build();

app.UseHttpsRedirection();
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FlightContext>();
    FlightDataSeeder.Seed(context);
}

app.MapControllers();
app.Run();