using IOTDeviceManagementSystem.Data;
using IOTDeviceManagementSystem.Interfaces;
using IOTDeviceManagementSystem.Models;
using Microsoft.Data.SqlClient;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace IOTDeviceManagementSystem.Repositories
{

    public class DeviceRepository : IDeviceRepository
    {
        private readonly EFCoreDbContext context;
        public DeviceRepository(EFCoreDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> AddDevice(Device device)
        {
            try
            {
                context.Devices.Add(device);

                int rowsAffected =await context.SaveChangesAsync();
                if (rowsAffected > 0)
                {
                    Logger.Logs($"[Success - {DateTime.Now:yyyy-MM-dd HH:mm:ss}] Device Added Successfully.");
                    return true;
                }
                else
                {
                    Logger.Logs($"[Failed - {DateTime.Now:yyyy-MM-dd HH:mm:ss}] Failed to add device.");
                    return false;
                }
            }

            catch (DbUpdateException ex)
            {
                Logger.Logs($"[Error - {DateTime.Now:yyyy-MM-dd HH:mm:ss}] Database update error {ex.Message}.");
                return false;
            }
            catch (Exception ex)
            {
                Logger.Logs($"[Error - {DateTime.Now:yyyy-MM-dd HH:mm:ss}] DB error {ex.Message}.");
                return false;
            }

        }

        public async Task<bool> DeleteDevice(string deviceID)
        {
            try
            {

                int rowsAffected =await context.Devices
                                                .Where(d => d.DeviceID == deviceID)
                                                .ExecuteDeleteAsync();
                        if (rowsAffected > 0)
                        {
                            Logger.Logs($"[Success - {DateTime.Now:yyyy-MM-dd HH:mm:ss}] Device Removed Successfully");
                            return true;
                        }
                        else
                        {
                            Logger.Logs($"[Warning - {DateTime.Now:yyyy-MM-dd HH:mm:ss}] No such record found");
                            return false;
                        }
                    }
            catch (DbUpdateException ex)
            {
                Logger.Logs($"[Error - {DateTime.Now:yyyy-MM-dd HH:mm:ss}] Database update error {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Logger.Logs($"[Error - {DateTime.Now:yyyy-MM-dd HH:mm:ss}] DB error {ex.Message}");
                return false;
            }
        }

        public async Task<List<Device>> RetrieveAllDevices()
        {
            try
            {
                return await context.Devices
                                    .OrderBy(x => x.DeviceID)
                                    .ToListAsync();
                    ;
            }
            catch (DbUpdateException ex)
            {
                Logger.Logs($"[Error - {DateTime.Now:yyyy-MM-dd HH:mm:ss}] Database update error {ex.Message}");
                return new List<Device>();
            }
            catch (Exception ex)
            {
                Logger.Logs($"[Error - {DateTime.Now:yyyy-MM-dd HH:mm:ss}] DB error {ex.Message}");
                return new List<Device>();
            }
        }

        public async Task<Device?> GetDeviceById(string deviceId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(deviceId))
                {
                    return null;
                }
                Device? device = await context.Devices.FindAsync(deviceId);
                return device;
            }
            catch (DbUpdateException ex)
            {
                Logger.Logs($"[Error - {DateTime.Now:yyyy-MM-dd HH:mm:ss}] DB lookup error {ex.Message}");
            }
            catch (Exception ex)
            {
                Logger.Logs($"[Errror - {DateTime.Now:yyyy-MM-dd HH:mm:ss}] DB error {ex.Message}");
            }
            return null;
        }
        

        public async Task<bool> UpdateDevice(Device device)
        {
            try
            {
                context.Devices.Update(device);
                int rowsAffected = await context.SaveChangesAsync();

                        if (rowsAffected > 0)
                        {
                            Logger.Logs($"[Success - {DateTime.Now:yyyy-MM-dd HH:mm:ss}] Updated Succesfully");
                            return true;

                        }
                        else
                        {
                            Logger.Logs($"[Warning - {DateTime.Now:yyyy-MM-dd HH:mm:ss}] No Updation");
                            return false;
                        }
                    }
            catch (DbUpdateException ex)
            {
                Logger.Logs($"[Error - {DateTime.Now:yyyy-MM-dd HH:mm:ss}] DB update error {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Logger.Logs($"[Error - {DateTime.Now:yyyy-MM-dd HH:mm:ss}] {ex.Message}");
                return false;
            }
        }
    }
 }

