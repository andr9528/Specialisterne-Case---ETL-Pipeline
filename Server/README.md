# Weather Server (C# API)

This project is the API layer for querying weather sensor data stored in the database.

It is designed to work together with the Python ETL project, which is responsible for collecting and storing the data.

---

## Overview

The server provides:

- REST API endpoints for querying weather data  
- Support for filtering, ordering, and complex queries  
- Swagger UI for exploring and testing endpoints  

---

## Getting Started

### 1. Build the Project

There are currently **no release builds available**, so the project must be built from source.

Navigate to the Server folder:

``` bash
cd Server
```

Then build the project:

``` bash
dotnet build
```

---

### 2. First Run (Expected Failure)

Staying inside the Server folder navigated into in the last step, run the server:

``` bash
dotnet run --project Weather.Server
```

On the first run, the application will **crash intentionally**.

This is expected behavior — it indicates that the required configuration is missing.

---

### 3. Configure Connection String

You must configure the database connection string in:

``` text
appsettings.secrets.json
```

On Windows, this file is typically located at:

``` text
C:\Users\<YourUser>\AppData\Local\Fang Software\Weather Api
```

#### Example configuration

Below is an example of how the file should look:

``` json
{
  "SecretsConfig": {
    "ConnectionString": "Host=localhost;Port=5432;Database=weather;Username=postgres;Password=YourPassword"
  }
}
```

> ⚠️ Important  
> The connection string must match the database used by the Python ETL project.  
> Otherwise, the API will not be able to retrieve any data.
> ⚠️ Note  
> Restarting the application **without updating the connection string will just cause it to crash again**.

---

### 4. Run the Server

After configuring the connection string, run the server again:

``` bash
dotnet run --project Server/Weather.Server
```

---

### 5. Access Swagger UI

Once the server is running, open your browser and navigate to:

``` bash
http://localhost:5027
```

This will open Swagger, where you can:

- View all available endpoints  
- Execute requests directly from the browser  
- Inspect request/response formats  

---

## Getting Familiar with the API

A great starting point is the `SearchableSamples` endpoints.

These endpoints:

- Provide ready-to-use query examples
- Query examples includes comments indicating expected return data on usage
- Show how to structure requests
- Indicate which endpoints they can be used with

Example endpoints include:

- `/api/SearchableSamples/GetSearchableBmeSamples`
- `/api/SearchableSamples/GetComplexSearchableBmeSamples`
- `/api/SearchableSamples/GetSearchableDmiSamples`
- `/api/SearchableSamples/GetComplexSearchableDmiSamples`

Each endpoint description explains where the returned samples can be used.

> 💡 Tip  
> Copy a sample from one of these endpoints and use it in the corresponding query endpoint.  
> This is the fastest way to understand how the API works.

---

## Technologies Used

This project is built using:

- **.NET** (C#)
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **PostgreSQL**
- **Swagger / OpenAPI**
- **Serilog (Logging)**

NuGet packages and versions are centrally managed in:

``` text
Directory.Packages.props
```

---

## Summary

- The server must be built manually (no releases available)  
- First run will fail until configuration is provided  
- Requires a valid PostgreSQL connection string  
- Swagger UI is available at `http://localhost:5027`  
- `SearchableSamples` endpoints are the best starting point for learning the API
