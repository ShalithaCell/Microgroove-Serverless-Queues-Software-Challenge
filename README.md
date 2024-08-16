# Azure Functions Queue Communication Project

This project implements an Azure Functions-based solution where two functions communicate via Azure Storage Queues. The solution is designed to be run locally using Visual Studio and the Azurite emulator.

## Table of Contents

- [Challenge Overview](#challenge-overview)
- [Solution Structure](#solution-structure)
- [Prerequisites](#prerequisites)
- [Important Note](#important-note)
- [Getting Started](#getting-started)
  - [1. Clone the Repository](#1-clone-the-repository)
  - [2. Build the Solution](#2-build-the-solution)
  - [3. Run the Solution](#3-run-the-solution)
- [Running Tests](#running-tests)
- [Project Structure](#project-structure)
- [Technologies Used](#technologies-used)

## Challenge Overview

The challenge is to build a C# Azure Functions-based solution where:

- **Function A** is triggered by an HTTP request containing a JSON payload with `FirstName` and `LastName` properties.
- **Function A** saves the received data to an in-memory SQLite database and publishes a message to an Azure Storage Queue.
- **Function B** is triggered by messages on the storage queue. It makes an HTTP call to a REST API to fetch SVG data associated with the concatenated `FirstName` and `LastName`.
- The SVG data is saved back to the database, associated with the original `FirstName` and `LastName`.



## Solution Structure

The solution separates Azure Functions triggers/entry points from the core business logic. This structure promotes better testing, scalability, and maintainability.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) with Azure development workload installed
- [Azurite](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azurite) (Azure Storage Emulator)

## Important Note

FunctionA is configured with Anonymous authentication level for testing purposes during local development. This allows for easier testing without authentication barriers. However, this setting will be updated to a more secure authentication level (e.g., Function or Admin) when the function is published to a production environment to ensure appropriate security measures are in place.

## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/ShalithaCell/Microgroove-Serverless-Queues-Software-Challenge.git
cd Microgroove-Serverless-Queues-Software-Challenge
```

### 2. Build the Solution
Open the solution in Visual Studio and build it to restore all necessary NuGet packages.

### 3. Run the Solution

To run the solution locally:

1. Ensure Azurite is running.
2. **Make sure you have selected the Azure Functions project (`Microgroove.Function.PersonNameService`) as the startup project** in Visual Studio. This ensures that the correct project is executed when you run the solution.
3. Press `F5` in Visual Studio to start the functions.

### 4. Making a Request

To trigger **Function A**, send an HTTP POST request to:

```bash
http://localhost:7191/api/FunctionA
```

With a JSON payload:

```json
{
  "FirstName": "John",
  "LastName": "Doe"
}
```

## Running Tests

To run the unit tests, you can use the Test Explorer in Visual Studio or the command line:

```bash
dotnet test
```

The tests include:

- Validating JSON payload.
- Checking database insertion.
- Verifying message queuing.
- Handling errors and conflicts.

## Project Structure


```plaintext
├── src
│   ├── Application
│   │   └── Microgroove.Application       # Core business logic and services
│   │       ├── Dependencies
│   │       ├── DTOs                      # Data Transfer Objects
│   │       ├── Services                  # Service Interfaces and Implementations
│   │       ├── Validators                # Input validation logic
│   │       └── ServiceCollectionExtensions.cs
│   ├── Domain
│   │   └── Microgroove.Domain            # Domain entities and repositories
│   │       ├── Dependencies
│   │       ├── Entities                  # Domain entities
│   │       └── Repositories              # Domain repository interfaces
│   ├── Functions
│   │   └── Microgroove.Function.PersonNameService  # Azure Functions implementation (Function A and Function B)
│   ├── Infrastructure
│   │   └── Microgroove.Infrastructure    # Data access and infrastructure
│   │       ├── Dependencies
│   │       ├── Persistence               # Database context and configuration
│   │       └── ServiceCollectionExtensions.cs
└── tests
    └── Microgroove.Function.Test         # Unit tests using xUnit and Moq
```

## Technologies Used

- C# / .NET 8
- Azure Functions
- Azure Storage Queues
- SQLite (In-Memory)
- XUnit and Moq (for unit testing)
- Azurite (Azure Storage Emulator)
- FluentValidation (for input validation)