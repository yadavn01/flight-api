# Flight Information API

A RESTful API for managing flight information, built with C# and .NET 8.  
Implements CRUD operations, search, validation, logging, and automated tests.

---

## Features

- CRUD endpoints for flights
- Search flights by airline, airport, date range, and status
- Input validation (DataAnnotations + FluentValidation)
- Error handling with proper HTTP status codes
- Logging using ILogger
- In-memory database (EF Core)
- Swagger/OpenAPI documentation
- SOLID architecture with service and validator layers
- Automated tests with xUnit and Moq

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- (Optional) [Visual Studio Code](https://code.visualstudio.com/) or another IDE

---

### Running the API

```sh
dotnet run --project FlightApi/FlightApi.csproj
```

- Swagger UI: `https://localhost:5187/swagger`

---

### Running the Tests

```sh
dotnet test FlightApiTest/FlightApiTest.csproj
```

---

## API Endpoints

- `GET    /api/flights`           — Retrieve all flights
- `GET    /api/flights/{id}`      — Retrieve a specific flight by ID
- `POST   /api/flights`           — Create a new flight
- `PUT    /api/flights/{id}`      — Update an existing flight
- `DELETE /api/flights/{id}`      — Delete a flight
- `GET    /api/flights/search`    — Search flights by criteria

---

## Project Structure

```
flight-api/
│
├── FlightApi/           # Main API project
│   ├── Controllers/
│   ├── Models/
│   ├── Services/
│   ├── Validators/
│   ├── Data/
│   └── Program.cs
│
├── FlightApiTest/       # Test project
│   └── FlightServiceTests.cs
│
├── FlightInformation.csv
├── FlightApi.sln
└── README.md
```

---

## Notes

- The in-memory database is seeded on startup (see `FlightDataSeeder`).
- All validation and business logic is handled in service and validator layers.
- Logging is enabled for all major operations.
- Modify or extend the API as needed for your use case.

---

## Author

Naman Yadav
