using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace IOTDeviceManagementSystem.Models
{
    public enum DeviceType
    {
        Gateway,            // Fixed-power central data transmission unit
        TelemetryModule,    // Battery-operated remote logger tracking system health
        DataLogger
    }
    public class Device
    {
        [Key]
        [StringLength(10)]
        public string DeviceID { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string DeviceName { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string DeviceType { get; set; } = string.Empty;

        [Required]
        [StringLength(40)]
        public string Location { get; set; } = string.Empty;

        [Required]
        [RegularExpression("Active|Inactive",ErrorMessage ="Status can be either Active or Inactive.")]
        public string Status { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<Telemetry> Telemetries { get; set; } = new List<Telemetry>();
    }
}
