using System;
using System.Collections.Generic;
using System.Text;
using IOTDeviceManagementSystem.Models;
namespace IOTDeviceManagementSystem.Services
{
    public interface IDeviceService
    {
        Task<bool> RegisterDeviceAsync(Device device);

        Task<bool> ModifyDevice(Device device);

        Task<bool> RemoveDevice(string deviceId);

        DeviceType? GetDeviceType();

        Task<Device?> GetDeviceById(string deviceId);
       
        Task<List<Device>> GetAllDevices();

        Task<bool> DeviceExists(string deviceId);

    }
}
