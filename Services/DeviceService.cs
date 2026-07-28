using IOTDeviceManagementSystem.Interfaces;
using IOTDeviceManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOTDeviceManagementSystem.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository repository;   
        public DeviceService(IDeviceRepository repository)
        {
            this.repository = repository;  
        }
        public DeviceType? GetDeviceType()
        {
            while (true)
            {
                Console.WriteLine("\n--- Select Device Type ---");
                Console.WriteLine("1. Gateway");
                Console.WriteLine("2. TelemetryModule");
                Console.WriteLine("3. DataLogger");
                Console.Write("Enter selection number (1-3): ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        return DeviceType.Gateway;
                    case "2":
                        return DeviceType.TelemetryModule;
                    case "3":
                        return DeviceType.DataLogger;
                    case var _ when string.IsNullOrWhiteSpace(choice):
                        Console.WriteLine("Exiting menu...");
                        return null; 
                    default:
                        Console.WriteLine("Invalid selection! Please enter 1, 2, or 3.");
                        break;
                }
            }
        }
        private bool IsValidDevice(Device device)
        {
            if (string.IsNullOrWhiteSpace(device.DeviceID) ||
                    string.IsNullOrWhiteSpace(device.DeviceName) ||
                    string.IsNullOrWhiteSpace(device.DeviceType) ||
                    string.IsNullOrWhiteSpace(device.Location) ||
                    string.IsNullOrWhiteSpace(device.Status)  ||
                    !Enum.IsDefined(typeof(DeviceType),device.DeviceType)) 
            {
                return false;
            }
            if (device.Status != "Active" && device.Status != "Inactive")
            {
                Logger.Logs($"[Warning - {DateTime.Now:yyyy-MM-dd HH:mm:ss}] Invalid status.");
                return false;
            }
            return true;
        }

        public async Task<bool> RegisterDeviceAsync(Device device)
        {
            if (device == null || !IsValidDevice(device))
            {
                return false;
            }

            Device? exists = await repository.GetDeviceById(device.DeviceID);

            if (exists != null)
            {
                return false;
            }

            return await repository.AddDevice(device); 
        }
        public async Task<Device?> GetDeviceById(string deviceId)
        {
            if(string.IsNullOrWhiteSpace(deviceId))
            {
               return null;
            }
            return await repository.GetDeviceById(deviceId);

        }
        public async Task<bool> DeviceExists(string deviceId)
        {
            if (string.IsNullOrWhiteSpace(deviceId))
            {
                return false;
            }
            Device? device = await repository.GetDeviceById(deviceId);
            return device != null;
        }

        public async Task<List<Device>> GetAllDevices()
        {
            return await repository.RetrieveAllDevices();
        }

        public async Task<bool> ModifyDevice(Device device)
        {
            if(device == null || !IsValidDevice(device))
            {
                return false;
            }

            return await repository.UpdateDevice(device); 
        }

        public async Task<bool> RemoveDevice(string deviceId)
        {
            if (string.IsNullOrWhiteSpace(deviceId))
            {
                return false;
            }
            if (await repository.GetDeviceById(deviceId) == null)
            {
                return false;
            }
            return await repository.DeleteDevice(deviceId);
        }

    }
}
