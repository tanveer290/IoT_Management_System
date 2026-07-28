using IOTDeviceManagementSystem.Data;
using IOTDeviceManagementSystem.Interfaces;
using IOTDeviceManagementSystem.Models;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace IOTDeviceManagementSystem.Repositories
{
    public class TelemetryRepository : ITelemetryRepository
    {
        private readonly EFCoreDbContext context;

        public TelemetryRepository(EFCoreDbContext context)
        {
            this.context = context;
        }
        public async Task<bool> AddTelemetry(Telemetry telemetry)
        {
            try
            {
                        await context.Telemetries.AddAsync(telemetry);
                        int rowsAffected = await context.SaveChangesAsync();
                        if (rowsAffected > 0)
                        {
                            Logger.Logs($"[Success - {DateTime.Now:yyyy-MM-dd HH:mm:ss}] Device Added Successfully");
                            return true;
                        }
                        else
                        {
                            Logger.Logs($"[Error - {DateTime.Now:yyyy-MM-dd HH:mm:ss}] Failed to add device");
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
                Logger.Logs($"[Error - {DateTime.Now:yyyy-MM-dd HH:mm:ss}] DB error {ex.Message}");
                return false;
            }

        }
        public async Task<List<Telemetry>> GetAllTelemetry()
        {
            try
            {
                return await context.Telemetries
                             .OrderBy(t => t.DeviceID)
                             .ThenByDescending(t => t.TelemetryID)
                             .ToListAsync<Telemetry>();
            }
            
            catch (DbUpdateException ex)
            {
                Logger.Logs($"[Error - {DateTime.Now:yyyy-MM-dd HH:mm:ss}] DB updation error {ex.Message}");
                return [];
            }
            catch (Exception ex)
            {
                Logger.Logs($"[Error - {DateTime.Now:yyyy-MM-dd HH:mm:ss}] DB error {ex.Message}");
                return [];
            }
        }
        public async Task<List<Telemetry>> GetTelemetryByDeviceId(string deviceId)
        {
            try
            {
                return await context.Telemetries
                             .Where(t => t.DeviceID == deviceId)
                             .OrderBy(t => t.RecordedAt)
                             .ToListAsync<Telemetry>();
            }

            catch (DbUpdateException ex)
            {
                Logger.Logs($"[Error - {DateTime.Now:yyyy-MM-dd HH:mm:ss}] DB update error {ex.Message}");
                return [];
            }
            catch (Exception ex)
            {
                Logger.Logs($"[Error - {DateTime.Now:yyyy-MM-dd HH:mm:ss}]DB error {ex.Message}");
                return [];
            }
        }
        public async Task<Telemetry?> GetLatestTelemetry(string deviceId)
        {
            if (string.IsNullOrWhiteSpace(deviceId))
            {
                return null;
            }
            try
            {
                return  await context.Telemetries
                             .Where(t => t.DeviceID == deviceId)
                             .OrderByDescending(t => t.RecordedAt)
                             .FirstOrDefaultAsync();
                }              
            

            catch (DbUpdateException ex)
            {
                Logger.Logs($"[Error - {DateTime.Now:yyyy-MM-dd HH:mm:ss}] DB update error {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Logger.Logs($"[Error - {DateTime.Now:yyyy-MM-dd HH:mm:ss}] DB error {ex.Message}");
                return null;
            }
        }


        public async Task<bool> DeleteTelemetry(string deviceID)
        {
            try
            {

                int deletedRows= await context.Telemetries
                                              .Where(t => t.DeviceID == deviceID)
                                              .ExecuteDeleteAsync();
                if (deletedRows > 0)
                {
                    return true;
                }
                return false;
            }
            catch (SqlException ex)
            {
                Logger.Logs($"[Error - {DateTime.Now:yyyy-MM-dd HH:mm:ss}] SQL error {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Logger.Logs($"[Error - {DateTime.Now:yyyy-MM-dd HH:mm:ss}] DB error {ex.Message}");
                return false;
            }
        }


    }
}
