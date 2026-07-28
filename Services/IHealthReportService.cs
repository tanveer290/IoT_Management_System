using System;
using System.Collections.Generic;
using System.Text;

namespace IOTDeviceManagementSystem.Services
{
    public interface IHealthReportService
    {
        Task<List<string>> GenerateHealthReport();
    }
}
