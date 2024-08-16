# Azure Functions Queue Communication Project

This project implements an Azure Functions-based solution where two functions communicate via Azure Storage Queues. The solution is designed to be run locally using Visual Studio and the Azurite emulator.

## Table of Contents

- [Challenge Overview](#challenge-overview)
- [Solution Structure](#solution-structure)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
  - [1. Clone the Repository](#1-clone-the-repository)
  - [2. Build the Solution](#2-build-the-solution)
  - [3. Run the Solution](#3-run-the-solution)
- [Running Tests](#running-tests)
- [Project Structure](#project-structure)
- [Technologies Used](#technologies-used)
- [Key Features](#key-features)

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
2. Press `F5` in Visual Studio to start the functions.

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

## Running Tests