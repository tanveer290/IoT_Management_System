using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IOTDeviceManagementSystem.Models
{
    public class Telemetry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TelemetryID { get; set; }

        [Required]
        [StringLength(10)]
        public string DeviceID { get; set; } = string.Empty;

        [Column(TypeName ="decimal(5,2)")]
        public decimal Temperature { get; set; }

        [Column(TypeName ="decimal(4,2)")]
        public decimal Humidity { get; set; }

        [Range(0,100)]
        public int BatteryLevel { get; set; }

        public DateTime RecordedAt { get; set; }

        [ForeignKey(nameof(DeviceID))]
        public virtual Device? Device { get; set; }

        public static implicit operator Telemetry(List<Telemetry> v)
        {
            throw new NotImplementedException();
        }
    }
}
