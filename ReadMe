# IoT Device Management System

A backend application developed using **C# (.NET)** for managing IoT devices and their telemetry data. The application provides device registration, telemetry recording, health monitoring, and reporting using a layered architecture following SOLID principles.

---

# Features

- Register IoT devices
- Update device information
- Delete devices
- Retrieve all registered devices
- Record telemetry data
- Retrieve telemetry history for a device
- Retrieve latest telemetry
- Generate device health reports
- Input validation in Service Layer
- Repository Pattern
- Entity Framework Core
- SQL Server database
- Unit Testing using xUnit and Moq
- Exception handling and logging

---

# Technologies Used

- C#
- .NET
- Entity Framework Core
- SQL Server
- xUnit
- Moq
- LINQ
- Asynchronous Programming (async/await)

---

# Project Architecture

```
Presentation Layer
        │
        ▼
Service Layer
        │
        ▼
Repository Layer
        │
        ▼
Entity Framework Core
        │
        ▼
SQL Server
```

---

# Project Structure

```
IoTDeviceManagementSystem
│
├── Models
│      Device.cs
│      Telemetry.cs
│
├── Interfaces
│      IDeviceRepository.cs
│      ITelemetryRepository.cs
│      IDeviceService.cs
│      ITelemetryService.cs
│
├── Repositories
│      DeviceRepository.cs
│      TelemetryRepository.cs
│
├── Services
│      DeviceService.cs
│      TelemetryService.cs
│      HealthReportService.cs
│
├── Data
│      EFCoreDbContext.cs
│
├── Logger
│      Logger.cs
│
├── UnitTesting
│      DeviceServiceTest.cs
│      TelemetryServiceTest.cs
│      HealthReportServiceTest.cs
│
└── Program.cs
```

---

# Database Design

## Device

| Column | Type |
|----------|----------|
| DeviceID | string |
| DeviceName | string |
| DeviceType | string |
| Location | string |
| Status | string |
| CreatedDate | DateTime |

---

## Telemetry

| Column | Type |
|----------|----------|
| TelemetryID | int |
| DeviceID | string |
| Temperature | decimal |
| Humidity | int |
| BatteryLevel | int |
| RecordedAt | DateTime |

---

# Entity Relationship

```
Device
--------
DeviceID (PK)

      │
      │ 1
      │
      ▼

Telemetry
---------
TelemetryID (PK)
DeviceID (FK)
```

One device can have multiple telemetry records.

---

# Functionalities

## Device Management

- Register Device
- Update Device
- Delete Device
- Get Device By ID
- Retrieve All Devices

---

## Telemetry Management

- Record Telemetry
- Retrieve Telemetry History
- Retrieve Latest Telemetry

---

## Health Report

Generates device health based on latest telemetry.

### Healthy

- Battery ≥ 20%
- Temperature ≤ 45°C

Example

```
Device D001 : Healthy
```

---

### High Temperature

```
Temperature > 45°C
```

Example

```
Device D001 : High Temperature
```

---

### Low Battery

```
Battery < 20%
```

Example

```
Device D001 : Low Battery
```

---

### Multiple Alerts

Example

```
Device D001 : High Temperature | Low Battery
```

---

### No Telemetry

Example

```
Device D001 : No Telemetry Available
```

---

# Business Validations

## Device

- Device cannot be null
- DeviceID cannot be empty
- DeviceName cannot be empty
- DeviceType cannot be empty
- Location cannot be empty
- Duplicate DeviceID not allowed

---

## Telemetry

- Telemetry cannot be null
- DeviceID cannot be empty
- Device must exist before recording telemetry

---

# Exception Handling

Repository layer handles:

- Database update exceptions
- SQL exceptions
- Unexpected exceptions

All exceptions are logged using the Logger utility.

---

# Logging

Application logs:

- Device Registration
- Device Update
- Device Deletion
- Telemetry Recording
- Errors
- Health Report Generation

---

# Async Programming

The application uses asynchronous programming for database operations.

Examples:

```csharp
await repository.AddDevice(device);

await repository.AddTelemetry(telemetry);

await context.SaveChangesAsync();
```

Benefits

- Better scalability
- Non-blocking database operations
- Improved application responsiveness

---

# Unit Testing

Implemented using

- xUnit
- Moq

---

## DeviceService Tests

- Register valid device
- Register null device
- Register duplicate device
- Register invalid device
- Delete existing device
- Delete invalid device
- Update existing device
- Update invalid device

---

## TelemetryService Tests

- Record valid telemetry
- Record invalid telemetry
- Record null telemetry
- Device does not exist
- Retrieve latest telemetry
- Retrieve latest telemetry when none exists
- Retrieve telemetry history
- Retrieve empty telemetry history

---

## HealthReportService Tests

- Healthy device
- High temperature
- Low battery
- High temperature and low battery
- No telemetry available
- No registered devices

---

# Design Principles

- SOLID Principles
- Separation of Concerns
- Dependency Injection
- Repository Pattern
- Service Layer Pattern

---
# Sample Health Report

```
-------------------------------------------
Health Report
-------------------------------------------

Device ID      : D001
Device Name    : Temperature Sensor
Temperature    : 38°C
Battery Level  : 65%
Status         : Healthy

-------------------------------------------

Device ID      : D002
Temperature    : 52°C
Battery Level  : 14%
Status         : High Temperature | Low Battery

-------------------------------------------
```

---

# Learning Outcomes

Through this project, the following concepts were implemented:

- Entity Framework Core
- SQL Server
- Repository Pattern
- Service Layer Pattern
- Dependency Injection
- LINQ
- Async/Await
- Unit Testing with xUnit
- Mocking using Moq
- Exception Handling
- Logging
- SOLID Principles

---
