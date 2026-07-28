using IOTDeviceManagementSystem.Interfaces;
using IOTDeviceManagementSystem.Models;

namespace IOTDeviceManagementSystem.Services
{
    public class TelemetryService : ITelemetryService
    {
        private readonly ITelemetryRepository telemetryRepository;
        private readonly IDeviceRepository deviceRepository;
        public TelemetryService(ITelemetryRepository telemetryRepository, IDeviceRepository deviceRepository)
        {
            this.telemetryRepository=telemetryRepository;
            this.deviceRepository=deviceRepository;
        }

        private bool IsValidTelemetry(Telemetry telemetry)
        {
            if (string.IsNullOrWhiteSpace(telemetry.DeviceID))
            {
                return false;
            }
            if (0 > telemetry.BatteryLevel || telemetry.BatteryLevel > 100)
            {
                return false;
            }
            if (0 > telemetry.Humidity|| telemetry.Humidity > 100)
            {
                return false;
            }
            return true;
        }
        public async Task<bool> RecordTelemetry(Telemetry telemetry)
        {
            if (telemetry == null)
            {
                return false;
            }
            if (!IsValidTelemetry(telemetry))
            {
                return false;
            }
            Device? device = await deviceRepository.GetDeviceById(telemetry.DeviceID);
            if (device == null)
            {
                return false;
            }
            if (device.Status == "Inactive")
            {
                return false;
            }
            return await telemetryRepository.AddTelemetry(telemetry);
        }

        public async Task<List<Telemetry>> GetAllTelemetry()
        {
            return await telemetryRepository.GetAllTelemetry();
        }

        public async Task<Telemetry?> GetLatestTelemetry(string deviceId)
        {
            if (string.IsNullOrWhiteSpace(deviceId))
            {
                return null;
            }
            return await telemetryRepository.GetLatestTelemetry(deviceId);

        }

        public async Task<List<Telemetry>> GetTelemetryHistory(string deviceId)
        {
            if (string.IsNullOrWhiteSpace(deviceId))
            {
                return new List<Telemetry>();
            }

            if (await deviceRepository.GetDeviceById(deviceId) == null)
            {
                return new List<Telemetry>();
            }

            return await telemetryRepository.GetTelemetryByDeviceId(deviceId);
        }
        public async Task<bool> RemoveTelemetry(string deviceId)
        {
            if (string.IsNullOrWhiteSpace(deviceId))
            {
                return false;
            }
            if (telemetryRepository.GetTelemetryByDeviceId(deviceId) == null)
            {
                return false;
            }
            return await telemetryRepository.DeleteTelemetry(deviceId);
        }


    }
}
