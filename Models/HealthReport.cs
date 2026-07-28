using System;
using System.Collections.Generic;
using System.Text;

namespace IOTDeviceManagementSystem.Models
{
        public class HealthReport
        {
            public string? DeviceID { get; set; }

            public string? DeviceName { get; set; }

            public decimal Temperature { get; set; }

            public decimal Humidity { get; set; }

            public int BatteryLevel { get; set; }

            public DateTime RecordedAt { get; set; }

            public string? HealthStatus { get; set; }
        }
}
