using System;
using System.Collections.Generic;
using System.Text;
using IOTDeviceManagementSystem.Models;

namespace IOTDeviceManagementSystem.Services
{
    public class HealthReportService : IHealthReportService
    {
        private readonly IDeviceService deviceService;
        private readonly ITelemetryService telemetryService;

        public HealthReportService(IDeviceService deviceService, ITelemetryService telemetryService)
        {
            this.deviceService = deviceService;
            this.telemetryService = telemetryService;
        }

        public async Task<List<string>> GenerateHealthReport()
        {

            List<string> healthReportList = new List<string>();
            List<Device> devices = await deviceService.GetAllDevices();
            List<string> deviceIds = new List<string>();

            foreach (var device in devices)
            {
                Telemetry? latestTelemetry = await telemetryService.GetLatestTelemetry(device.DeviceID);
                string healthReport = "";
                if (latestTelemetry != null)
                {
                    healthReport = $"\nDeviceId: {device.DeviceID}\nDeviceName: {device.DeviceName}\nTemperature: {latestTelemetry.Temperature}°C\nBatteryLevel: {latestTelemetry.BatteryLevel}%\nHealthStatus: ";
                    deviceIds.Add(device.DeviceID);
                    if (latestTelemetry.BatteryLevel < 20)
                    {
                        healthReport += "\nLow battery";
                    }
                    if (latestTelemetry.Temperature > 70)
                    {
                        healthReport += "\nHigh Temperature";
                    }
                    if (latestTelemetry.BatteryLevel >= 20 && latestTelemetry.Temperature <= 70)
                    {
                        healthReport += "Healthy";
                    }
                    healthReportList.Add(healthReport);
                }
                else
                {
                    healthReport = $"\nDeviceId : {device.DeviceID}\n" +
                                   $"DeviceName : {device.DeviceName}\n" +
                                   $"HealthStatus : No Telemetry Available";

                    healthReportList.Add(healthReport);
                }
            }
            if (healthReportList.Count == 0)
            {
                Logger.Logs($"[Warning - {DateTime.Now:yyyy-MM-dd HH:mm:ss}] No Telemetry Records found so health report can't be generated.");
                return [];          
            }
            Logger.Logs($"[Success - {DateTime.Now:yyyy-MM-dd HH:mm:ss}] Telemetry Records found for device IDs {string.Join(", ",deviceIds)} and Health Report was generated.");
            return healthReportList;
        }
    }
}
