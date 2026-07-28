using IOTDeviceManagementSystem;
using IOTDeviceManagementSystem.Data;
using IOTDeviceManagementSystem.Interfaces;
using IOTDeviceManagementSystem.Models;
using IOTDeviceManagementSystem.Repositories;
using IOTDeviceManagementSystem.Services;
using Microsoft.Data.SqlClient;


namespace project2
{
    public class Program
    {
        private static async Task RecordTelemetry(IDeviceService deviceService, ITelemetryService telemetryService)
        {
                Console.Write("Enter Device ID: ");
                string? deviceId = Console.ReadLine()?.Trim() ?? "";
                if (!await deviceService.DeviceExists(deviceId))
                {
                    Logger.Logs($"[Warning - {DateTime.Now:yyyy - MM - dd HH: mm: ss}] Device does n't Exists in device table so can't add its telemetry table.");
                    return;
                }

                Console.Write("Enter Temperature: ");
                Decimal temperature = decimal.Parse(Console.ReadLine()?.Trim() ?? "0");

                Console.Write("Enter Humidity: ");
                Decimal humidity = decimal.Parse(Console.ReadLine()?.Trim() ?? "0");

                Console.Write("Enter BatteryLevel: ");
                int batteryLevel = int.Parse(Console.ReadLine()?.Trim() ?? "0");

                Telemetry telemetry = new Telemetry
                {
                    DeviceID = deviceId ?? string.Empty,
                    Temperature = temperature,
                    Humidity = humidity,
                    BatteryLevel = batteryLevel,
                    RecordedAt = DateTime.Now
                };
                bool registered = await telemetryService.RecordTelemetry(telemetry);
                if (registered)
                {
                    return;
                }
                else
                {
                    return;
                }
            }
        
        private static void DisplayDevice(Device device)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"\nDevice ID     : {device.DeviceID}");
            Console.WriteLine($"Device Name   : {device.DeviceName}");
            Console.WriteLine($"Device Type   : {device.DeviceType}");
            Console.WriteLine($"Location      : {device.Location}");
            Console.WriteLine($"Status        : {device.Status}");
            Console.WriteLine($"Created Date  : {device.CreatedDate.ToString("dd-MM-yyyy")}");
            Console.WriteLine();
            Console.ResetColor();
        }
        private static void DisplayTelemetry(Telemetry telemetry)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"\nTelemetry ID  : {telemetry.TelemetryID}");
            Console.WriteLine($"Device ID     : {telemetry.DeviceID}");
            Console.WriteLine($"Temperature   : {telemetry.Temperature}°C");
            Console.WriteLine($"Humidity      : {telemetry.Humidity}%");
            Console.WriteLine($"Battery Level : {telemetry.BatteryLevel}%");
            Console.WriteLine($"RecordedAt    : {telemetry.RecordedAt.ToString("dd-MM-yyyy HH:mm:ss")}");
            Console.WriteLine();
            Console.ResetColor();
        }
        private static async Task GetLatestTelemetry(ITelemetryService telemetryService)
        {
                Console.Write("Enter Device ID: ");
                string? deviceId = Console.ReadLine()?.Trim() ?? "";

                Telemetry? latestTelemetry = await telemetryService.GetLatestTelemetry(deviceId);

                if (latestTelemetry == null)
                {
                    Logger.Logs($"[Warning - {DateTime.Now:yyyy - MM - dd HH: mm: ss}] No Telemetry found with device ID: {deviceId}.");
                    return;
                }
                DisplayTelemetry(latestTelemetry);
            return;
        }

        private static async Task RegisterDeviceAsync(IDeviceService deviceService) 
        {
                Console.Write("Enter Device ID: ");

                string? deviceId = Console.ReadLine()?.Trim() ?? "";
                if (await deviceService.DeviceExists(deviceId))
                {
                    Logger.Logs($"[Warning - {DateTime.Now:yyyy - MM - dd HH: mm: ss}] Device already Exists.");
                    return;
                }

                Console.Write("Enter Device Name: ");
                string? deviceName = Console.ReadLine()?.Trim();

                Console.Write("Enter Device Type: ");
                string? deviceType = deviceService.GetDeviceType().ToString();

                Console.Write("Enter Location: ");
                string? location = Console.ReadLine()?.Trim();

                string? status;

                while (true)
                {
                    Console.Write($"Enter Device Status (Active or Inactive): ");
                    status = Console.ReadLine()?.Trim();

                    if (!string.IsNullOrWhiteSpace(status))
                    {
                        if (status.Equals("Active", StringComparison.OrdinalIgnoreCase) ||
                            status.Equals("Inactive", StringComparison.OrdinalIgnoreCase))
                        {
                            status = char.ToUpper(status[0]) + status.Substring(1).ToLower();
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid Status,status can be either Active or Inactive");
                        }
                    }
                }
                Device device = new Device
                {
                    DeviceID = deviceId ?? string.Empty,
                    DeviceName = deviceName ?? string.Empty,
                    DeviceType = deviceType ?? string.Empty,
                    Location = location ?? string.Empty,
                    Status = status ?? string.Empty,
                    CreatedDate = DateTime.Now
                };
                bool registered = await deviceService.RegisterDeviceAsync(device);
                if (registered)
                {
                    return;
                }
                else
                {
                    return;
                }
            }
        private static async Task UpdateDevice(IDeviceService deviceService)
        {
                Console.Write("Enter Device ID: ");
                string? deviceId = Console.ReadLine()?.Trim() ?? "";
                Device? device = await deviceService.GetDeviceById(deviceId);

                if (device != null)
                {

                    Console.Write($"Current Device Name : {device.DeviceName}\r\nEnter new name (Leave blank to keep current):");
                    string? name = Console.ReadLine()?.Trim();
                    device.DeviceName = string.IsNullOrWhiteSpace(name)
                            ? device.DeviceName
                            : name;

                    Console.Write($"Current Device Type : {device.DeviceType}\r\nEnter new DeviceType (Leave blank to keep current):");
                    string? type = deviceService.GetDeviceType().ToString();
                    device.DeviceType = string.IsNullOrWhiteSpace(type)
                            ? device.DeviceType
                            : type;

                    Console.Write($"Current Location: {device.Location}\r\nEnter new Location (Leave blank to keep current):");
                    string? location = Console.ReadLine()?.Trim();
                    device.Location = string.IsNullOrWhiteSpace(location)
                            ? device.Location
                            : location;

                    while (true)
                    {
                        Console.Write($"Current Status : {device.Status}\r\nEnter updated status (Active or Inactive) (Leave blank to keep current):");
                        string? status = Console.ReadLine()?.Trim();

                        if (!string.IsNullOrWhiteSpace(status))
                        {
                            if (status.Equals("Active", StringComparison.OrdinalIgnoreCase) ||
                                status.Equals("Inactive", StringComparison.OrdinalIgnoreCase))
                            {
                                device.Status = char.ToUpper(status[0]) + status.Substring(1).ToLower();
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Invalid Status");
                            }
                        }
                    }
                    bool updatedDevice = await deviceService.ModifyDevice(device);
                    if (updatedDevice)
                    {
                        Logger.Logs($"[Success - {DateTime.Now:yyyy-MM-dd HH:mm:ss}] Device Updated Successfully.");
                        return;
                    }
                    else
                    {
                        Logger.Logs($"[Error - {DateTime.Now:yyyy-MM-dd HH:mm:ss} Error While updating");
                        return;
                    }
                }
                else
                {
                    Logger.Logs($"[Warning - {DateTime.Now:yyyy-MM-dd HH:mm:ss} Device ID Not Found");
                    return;
                }

            }
       
        private static async Task DeleteDevice(IDeviceService deviceService,ITelemetryService telemetryService)
        {
                Console.Write("Enter Device ID: ");
                string? deviceId = Console.ReadLine()?.Trim() ?? "";

                if(!await deviceService.DeviceExists(deviceId))
                {
                    Logger.Logs($"[Failed - {DateTime.Now:yyyy-MM-dd HH:mm:ss}] DeviceID does not exists in the table.");
                    return;
                }

                Console.Write("Are you sure? (Y/N): ");
                string? confirmation = Console.ReadLine()?.Trim();

                if (!string.Equals(confirmation, "Y", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Deletion cancelled.");
                    return;
                }

                bool removedDevice =await deviceService.RemoveDevice(deviceId);
            if (removedDevice)
            {
                string log = $"[Success - {DateTime.Now:yyyy-MM-dd HH:mm:ss}] Device {deviceId} deleted from the table Devices";
                List<Telemetry> telemetry = await telemetryService.GetTelemetryHistory(deviceId);
                if (telemetry != new List<Telemetry>())
                {
                    log+= " and Telemetries.";
                    await telemetryService.RemoveTelemetry(deviceId);
                    Logger.Logs(log);
                }
            }
            else
            {
                Logger.Logs($"[Failed - {DateTime.Now:yyyy-MM-dd HH:mm:ss}] Failed to delete from the table.");
                return;
            }
            }
         
        static async Task Main(String[] args)
        {
            EFCoreDbContext context = new EFCoreDbContext();

            IDeviceRepository devicerepository = new DeviceRepository(context);


            ITelemetryRepository telemetryRepository = new TelemetryRepository(context);

            IDeviceService deviceService = new DeviceService(devicerepository);

            ITelemetryService telemetryService = new TelemetryService(telemetryRepository,devicerepository);

            IHealthReportService healthReportService = new HealthReportService(deviceService,telemetryService);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==========================================\r\n      IoT Device Management System\r\n==========================================\r\n\r\n" +
                               "1. Register Device\r\n" +
                               "2. Update Device\r\n" +
                               "3. Delete Device\r\n" +
                               "4. Find Device\r\n" +
                               "5. View All Devices\r\n\r\n" +
                               "6. Record Telemetry\r\n" +
                               "7. View Telemetry History\r\n" +
                               "8. View Latest Telemetry\r\n" +
                               "9. View All Telemetry\r\n\r\n" +
                               "10. Generate Health Report\r\n\r\n" +
                               "11. Exit\r\n");
            Console.ResetColor();

            while (true)
            {
                Console.Write("\nEnter your Choice: ");
                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Invalid Choice");
                    continue;
                }
                switch (choice)
                {
                    case 1:
                         await RegisterDeviceAsync(deviceService);
                        break;
                    case 2:
                        await UpdateDevice(deviceService);
                        break;
                    case 3:
                        await DeleteDevice(deviceService,telemetryService);
                        break;
                    case 4:
                            Console.Write("Enter Device ID: ");
                            string deviceId = Console.ReadLine()?.Trim() ?? "";
                            Device? device = await deviceService.GetDeviceById(deviceId);
                            if (device == null)
                            {
                                Logger.Logs($"[Failed - {DateTime.Now:yyyy-MM-dd HH:mm:ss}] DeviceID does not exists in the table.");
                                break;
                            }
                            DisplayDevice(device);
                        break;
                    case 5:
                            List<Device> devices = await deviceService.GetAllDevices();
                            Console.WriteLine($"\nTotal Devices : {devices.Count}\n");
                            if (devices.Count == 0)
                            {
                                Console.WriteLine("No device data found.");
                                break;
                            }
                            foreach (Device devicedata in devices)
                            {
                                DisplayDevice(devicedata);
                            }
                        break;
                    case 6:
                        await RecordTelemetry(deviceService, telemetryService);
                        break;
                    case 7:
                            Console.Write("Enter Device ID: ");
                            string deviceID = Console.ReadLine()?.Trim() ?? "";
                            List<Telemetry> telemetryHistory = await telemetryService.GetTelemetryHistory(deviceID);
                            if (telemetryHistory.Count == 0)
                            {
                                Logger.Logs($"[Warning - {DateTime.Now:yyyy-MM-dd HH:mm:ss}] No Telemtry Record found with this DeviceID {deviceID}");
                                break;
                            }
                            Console.WriteLine($"\nTotal Records : {telemetryHistory.Count}\n");
                            foreach (Telemetry telemetry in telemetryHistory)
                            {
                                DisplayTelemetry(telemetry);
                            }
                        break;
                    case 8: await GetLatestTelemetry(telemetryService);
                            break;

                    case 9:
                            List<Telemetry> telemetries = await telemetryService.GetAllTelemetry();
                            Console.WriteLine($"\nTotal telemetry : {telemetries.Count}\n");
                            if (telemetries.Count == 0)
                            {
                                Logger.Logs($"[Warning - {DateTime.Now:yyyy-MM-dd HH:mm:ss}] No telemetry records found.");
                                break;
                            }
                            foreach (Telemetry telemetryData in telemetries)
                            {
                                DisplayTelemetry(telemetryData);
                            }
                        break;
                    case 10:
                            List<string> healthReports =await healthReportService.GenerateHealthReport();
                            if (healthReports.Count == 0)
                            {
                                Logger.Logs($"[Warning - {DateTime.Now:yyyy-MM-dd HH:mm:ss} No Telemetry Records found so health report can't be generated.");
                            }
                            foreach (string healthReport in healthReports)
                            {
                                Console.WriteLine($"{healthReport}");
                            }
                        break;
                    case 11:
                        Logger.DisplayLogs();
                        Console.WriteLine("Exiting....."); 
                            return;
                    default:
                        Console.WriteLine("Invalid Choice");
                        break;

                }
            }
        }
    }
}


