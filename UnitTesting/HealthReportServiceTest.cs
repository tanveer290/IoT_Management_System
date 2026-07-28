using Xunit;
using Moq;
using IOTDeviceManagementSystem.Models;
using IOTDeviceManagementSystem.Services;
using IOTDeviceManagementSystem.Interfaces;

namespace UnitTesting
{
    public class HealthReportServiceTest
    {
        private readonly Mock<IDeviceService> deviceServiceMock;
        private readonly Mock<ITelemetryService> telemetryServiceMock;
        private readonly HealthReportService healthReportService;

        public HealthReportServiceTest()
        {
            deviceServiceMock = new Mock<IDeviceService>();
            telemetryServiceMock = new Mock<ITelemetryService>();

            healthReportService = new HealthReportService(
                deviceServiceMock.Object,
                telemetryServiceMock.Object);
        }

        [Fact]
        public async Task GenerateHealthReport_WhenDeviceIsHealthy_ReturnsHealthyStatus()
        {
            Device device = new Device
            {
                DeviceID = "D001",
                DeviceName = "Temperature Sensor",
                DeviceType = "Gateway",
                Location = "Lab",
                Status = "Active",
                CreatedDate = DateTime.Now
            };

            List<Device> devices = new List<Device>
            {
                device
            };

            Telemetry telemetry = new Telemetry
            {
                TelemetryID = 1,
                DeviceID = "D001",
                Temperature = 44.3M,
                Humidity = 77,
                BatteryLevel = 52,
                RecordedAt = DateTime.Now
            };

            deviceServiceMock
                .Setup(x => x.GetAllDevices())
                .ReturnsAsync(devices);

            telemetryServiceMock
                .Setup(x => x.GetLatestTelemetry(device.DeviceID))
                .ReturnsAsync(telemetry);

            List<string> result = await healthReportService.GenerateHealthReport();

            Assert.Single(result);
            Assert.Contains("Healthy", result[0]);
        }
        [Fact]
        public async Task GenerateHealthReport_WhenDeviceBatteryLessthan20_ReturnsLowBattery()
        {
            Device device = new Device
            {
                DeviceID = "D001",
                DeviceName = "Temperature Sensor",
                DeviceType = "Gateway",
                Location = "Lab",
                Status = "Active",
                CreatedDate = DateTime.Now
            };

            List<Device> devices = new List<Device>
            {
                device
            };

            Telemetry telemetry = new Telemetry
            {
                TelemetryID = 1,
                DeviceID = "D001",
                Temperature = 44.3M,
                Humidity = 77,
                BatteryLevel = 17,
                RecordedAt = DateTime.Now
            };

            deviceServiceMock
                .Setup(x => x.GetAllDevices())
                .ReturnsAsync(devices);

            telemetryServiceMock
                .Setup(x => x.GetLatestTelemetry(device.DeviceID))
                .ReturnsAsync(telemetry);

            List<string> result = await healthReportService.GenerateHealthReport();

            Assert.Single(result);
            Assert.Contains("Low battery", result[0]);
        }
        [Fact]
        public async Task GenerateHealthReport_WhenDeviceTemperatureGreaterThan70_ReturnsHighTemperature()
        {
            Device device = new Device
            {
                DeviceID = "D001",
                DeviceName = "Temperature Sensor",
                DeviceType = "Gateway",
                Location = "Lab",
                Status = "Active",
                CreatedDate = DateTime.Now
            };

            List<Device> devices = new List<Device>
            {
                device
            };

            Telemetry telemetry = new Telemetry
            {
                TelemetryID = 1,
                DeviceID = "D001",
                Temperature = 77.3M,
                Humidity = 77,
                BatteryLevel = 17,
                RecordedAt = DateTime.Now
            };

            deviceServiceMock
                .Setup(x => x.GetAllDevices())
                .ReturnsAsync(devices);

            telemetryServiceMock
                .Setup(x => x.GetLatestTelemetry(device.DeviceID))
                .ReturnsAsync(telemetry);

            List<string> result = await healthReportService.GenerateHealthReport();

            Assert.Single(result);
            Assert.Contains("High Temperature", result[0]);
        }
        [Fact]
        public async Task GenerateHealthReport_WhenDeviceBatteryLessthan20andHighTemperature_ReturnsLowBatteryandHighTemperature()
        {
            Device device = new Device
            {
                DeviceID = "D001",
                DeviceName = "Temperature Sensor",
                DeviceType = "Gateway",
                Location = "Lab",
                Status = "Active",
                CreatedDate = DateTime.Now
            };

            List<Device> devices = new List<Device>
            {
                device
            };

            Telemetry telemetry = new Telemetry
            {
                TelemetryID = 1,
                DeviceID = "D001",
                Temperature = 77.3M,
                Humidity = 77,
                BatteryLevel = 17,
                RecordedAt = DateTime.Now
            };

            deviceServiceMock
                .Setup(x => x.GetAllDevices())
                .ReturnsAsync(devices);

            telemetryServiceMock
                .Setup(x => x.GetLatestTelemetry(device.DeviceID))
                .ReturnsAsync(telemetry);

            List<string> result = await healthReportService.GenerateHealthReport();

            Assert.Single(result);
            Assert.Contains("Low battery", result[0]);
            Assert.Contains("High Temperature", result[0]);
        }
        [Fact]
        public async Task GenerateHealthReport_WhenNoTelemetryAvailableforDeviceId_ReturnsNotelemetryAvilable()
        {
            Device device = new Device
            {
                DeviceID = "D001",
                DeviceName = "Temperature Sensor",
                DeviceType = "Gateway",
                Location = "Lab",
                Status = "Active",
                CreatedDate = DateTime.Now
            };

            List<Device> devices = new List<Device>
            {
                device
            };

            Telemetry? telemetry = null;

            deviceServiceMock
                .Setup(x => x.GetAllDevices())
                .ReturnsAsync(devices);

            telemetryServiceMock
                .Setup(x => x.GetLatestTelemetry(device.DeviceID))
                .ReturnsAsync(telemetry);

            List<string> result = await healthReportService.GenerateHealthReport();

            Assert.Single(result);
            Assert.Contains("D001", result[0]);
            Assert.Contains("No Telemetry Available", result[0]);
        }
        [Fact]
        public async Task GenerateHealthReport_WhenDeviceisNull_ReturnsEmptyList()
        {

            List<Device> devices = new List<Device>();

            deviceServiceMock
                .Setup(x => x.GetAllDevices())
                .ReturnsAsync(devices);

            List<string> result = await healthReportService.GenerateHealthReport();

            Assert.Empty(result);

            telemetryServiceMock.Verify(
                x => x.GetLatestTelemetry(It.IsAny<string>()),
                Times.Never);
        }
    }
}