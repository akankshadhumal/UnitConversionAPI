# Unit Conversion API

A REST API built with **C# and ASP.NET Core Web API >NET 10.0.401** that
converts numeric values between supported measurement units. The project
separates HTTP request handling from conversion logic and validates
requests before returning a result.

## Project Overview

The API accepts a value, a source unit, and a target unit. It converts
the value when both units belong to the same category and returns a
structured JSON response.

### Supported Categories and Units

  Category      Supported unit symbols
  ------------- -----------------------------------------
  Length        `m`, `km`, `cm`, `mm`, `ft`, `in`, `mi`
  Mass          `kg`, `g`, `mg`, `lb`, `oz`
  Temperature   `c`, `f`, `k`

The current implementation supports **15 units across 3 categories**.
Unit symbols are matched without case sensitivity.

## Technology Stack

-   **Language:** C#
-   **Framework:** ASP.NET Core Web API
-   **Target Framework:** .NET 10
-   **API format:** REST with JSON request and response bodies
-   **API documentation:** OpenAPI JSON in the Development environment
-   **IDE:** Visual Studio 2026

## Key Features

-   Convert values between supported length, mass, and temperature
    units.
-   Use one endpoint for all supported conversion categories.
-   Validate that the input is finite and that source and target units
    are provided and supported.
-   Reject conversions between incompatible categories, such as metres
    and kilograms.
-   Return `400 Bad Request` with an error message for invalid
    conversion requests.
-   Keep conversion logic in a service class separate from the
    controller.
-   Register the conversion service through dependency injection.
-   Include sample requests in `UnitConversionAPI.http` for testing from Visual
    Studio.

## API Endpoint

### Convert a value

`POST /api/conversions`

**Request body**

``` json
{
  "value": 25,
  "fromUnit": "c",
  "toUnit": "f"
}
```

**Example successful response**

``` json
{
  "originalValue": 25,
  "fromUnit": "c",
  "convertedValue": 77.0,
  "toUnit": "f",
  "category": "Temperature"
}
```

The exact decimal representation may vary slightly because the
implementation uses C# `double` arithmetic.

**Example error response**

A request that tries to convert between different categories, such as
metres (`m`) and kilograms (`kg`), returns HTTP `400 Bad Request`:

``` json
{
  "error": "Source and target units must belong to the same category."
}
```

## How the Conversion Works

Each unit is defined by its category, conversion factor, and optional
offset. The service converts the input into the category's base
representation and then converts that value into the target unit.

1.  Validate the request and unit symbols.
2.  Confirm that the source and target units belong to the same
    category.
3.  Convert the input using the source unit's factor and offset.
4.  Convert the intermediate value into the target unit.
5.  Return the original value, converted value, units, and category.

## Project Structure

``` text
UnitConversionAPI/
├── Controllers/
│   └── ConversionsController.cs   Handles POST /api/conversions
├── Data/
│   └── UnitDefinition.cs          Stores unit category, factor, and offset
├── Models/
│   ├── ConversionRequest.cs       Request DTO
│   └── ConversionResponse.cs.cs   Response DTO
├── Services/
│   └── ConversionService.cs       Unit definitions and conversion logic
├── Properties/
│   └── launchSettings.json        Local HTTP/HTTPS launch profiles
├── Program.cs                     App configuration and dependency injection
├── appsettings.json               Application settings
├── UnitConversionAPI.http                   Sample API requests
├── UnitConversionAPI.csproj                 Project dependencies and .NET target
└── UnitConversionAPI.sln                    Visual Studio solution
```

## Prerequisites

-   [.NET 10.0.401 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
-   Visual Studio 2026 or another IDE that supports .NET 10.0.401
-   Git (if cloning the repository)

Check the installed SDKs:

``` bash
dotnet --list-sdks
```

## Run the Project Locally

### Using Visual Studio

1.  Open `UnitConversionAPI.csproj` directly in Visual Studio.
2.  Run the project using the `http` launch profile.
3.  The configured HTTP address is `http://localhost:5197`.

**Note:** The included `UnitConversionAPI.sln` references a separate
`UnitConversion.Api.Tests` project that was not present in the uploaded
ZIP. To make the solution build cleanly, include that test project or
remove its stale reference from the solution. Opening `UnitConversionAPI.csproj`
directly avoids that missing solution reference.

### Using the .NET CLI

Open a terminal in the folder containing `UnitConversionAPI.csproj` and run:

``` bash
dotnet restore
dotnet build UnitConversionAPI.csproj
dotnet run --launch-profile http
```

The API will listen at:

`http://localhost:7018`

The OpenAPI JSON document is mapped in the Development environment at:

`http://localhost:7018/openapi/v1.json`

## Test the API

You can use the included `UnitConversionAPI.http` file in Visual Studio, or send a
request with a REST client such as Postman.

Example using `curl`:

``` bash
curl -X POST "http://localhost:7018/api/conversions" \
  -H "Content-Type: application/json" \
  -d "{\"value\":25,\"fromUnit\":\"c\",\"toUnit\":\"f\"}"
```

The HTTP request file includes examples for: - Metres to feet - Celsius
to Fahrenheit - Kilograms to pounds - An invalid cross-category
conversion

## Current Scope and Future Improvements

The current version focuses on core conversion functionality and
includes a limited set of units. Possible next steps include:

-   Add more units and conversion categories.
-   Add automated unit and integration tests to the repository.
-   Add configurable rounding and more explicit request validation.
-   Improve numeric precision and response formatting.
-   Expand API documentation and add continuous integration checks.

## Summary

This project demonstrates foundational .NET Web API development,
including REST endpoint design, request and response DTOs, dependency
injection, separation of concerns, unit conversion logic, and input
validation.
