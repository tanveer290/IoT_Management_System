using IOTDeviceManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOTDeviceManagementSystem.Interfaces
{
    public interface IDeviceRepository
    {
        Task<bool> AddDevice(Device device);

        Task<bool> UpdateDevice(Device device);

        Task<bool> DeleteDevice(string deviceID);

        Task<Device?> GetDeviceById(string deviceId);

        Task<List<Device>> RetrieveAllDevices();
    }
}
