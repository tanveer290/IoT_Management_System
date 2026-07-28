using IOTDeviceManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOTDeviceManagementSystem.Interfaces
{
    public interface ITelemetryRepository
    {
            Task<bool> AddTelemetry(Telemetry telemetry);

            Task<List<Telemetry>> GetAllTelemetry();

            Task<List<Telemetry>> GetTelemetryByDeviceId(string deviceId);

            Task<Telemetry?> GetLatestTelemetry(string deviceId);

            Task<bool> DeleteTelemetry(string deviceId);
    }
}
