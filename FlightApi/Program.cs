using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//EF Core with an in-memory database
builder.Services.AddDbContext<FlightContext>(options =>
options.UseInMemoryDatabase("FlightDb"));
builder.Services.AddScoped<FlightValidator>();
builder.Services.AddScoped<IFlightService, FlightService>();
builder.Services.AddControllers();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Services.AddSwaggerGen();
var app = builder.Build();

app.UseHttpsRedirection();
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FlightContext>();
    FlightDataSeeder.Seed(context);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); 
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();