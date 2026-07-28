using System;
using System.Collections.Generic;
using System.Text;
using IOTDeviceManagementSystem.Models;
namespace IOTDeviceManagementSystem.Services
{
    public interface ITelemetryService
    {
        Task<bool> RecordTelemetry(Telemetry telemetry);

        Task<List<Telemetry>> GetTelemetryHistory(string deviceId);

        Task<Telemetry?> GetLatestTelemetry(string deviceId);

        Task<List<Telemetry>> GetAllTelemetry();

        Task<bool> RemoveTelemetry(string  deviceId);
    }
}
