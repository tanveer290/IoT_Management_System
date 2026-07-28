using IOTDeviceManagementSystem.Interfaces;
using IOTDeviceManagementSystem.Models;
using IOTDeviceManagementSystem.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTesting
{
    public class TelemetryServiceTest
    {
        private readonly Mock<IDeviceRepository> devicemockRepository;
        private readonly DeviceService deviceService;
        private readonly Mock<ITelemetryRepository> telemetrymockRepository;
        private readonly TelemetryService telemetryService;

        public TelemetryServiceTest()
        {
            devicemockRepository = new Mock<IDeviceRepository>();
            telemetrymockRepository = new Mock<ITelemetryRepository>();

            telemetryService = new TelemetryService(
                telemetrymockRepository.Object,
                devicemockRepository.Object);
        }
        private Device CreateValidDevice()
        {
            return new Device
            {
                DeviceID = "D001",
                DeviceName = "Temperature Sensor",
                DeviceType = "Gateway",
                Location = "Lab",
                Status = "Active",
                CreatedDate = DateTime.Now
            };
        }

        //Device is valid and does n't exists in the table.
        [Fact]
        public async Task AddTelemetry_WhenTelemetryIsValid_ReturnsTrue()
        {
            // Arrange
            Device device = CreateValidDevice();
            Telemetry telemetry = new Telemetry
            {
                TelemetryID = 1,
                DeviceID = "D001",
                Temperature = 44.3M,
                Humidity = 77,
                BatteryLevel = 17,
                RecordedAt = DateTime.Now
            };

            devicemockRepository
                .Setup(r => r.GetDeviceById(device.DeviceID))
                .ReturnsAsync(device);

            telemetrymockRepository
                .Setup(r => r.AddTelemetry(telemetry))
                .ReturnsAsync(true);

            // Act
            bool result = await telemetryService.RecordTelemetry(telemetry);

            // Assert
            Assert.True(result);

            telemetrymockRepository.Verify(
                r => r.AddTelemetry(telemetry),
                Times.Once);
        }
        [Fact]
        public async Task AddTelemetry_WhenTelemetryIsINull_ReturnsFalse()
        {
            // Arrange
            Device device = CreateValidDevice();
            Telemetry? telemetry = null;

            devicemockRepository
                .Setup(r => r.GetDeviceById(device.DeviceID))
                .ReturnsAsync(device);

            telemetrymockRepository
                .Setup(r => r.AddTelemetry(telemetry))
                .ReturnsAsync(false);

            // Act
            bool result = await telemetryService.RecordTelemetry(telemetry);

            // Assert
            Assert.False(result);

            telemetrymockRepository.Verify(
                r => r.AddTelemetry(telemetry),
                Times.Never);
        }

        [Theory]
        [InlineData("", 44.3, 77, 17)]
        [InlineData("D001", 44.3, -1, 17)]
        [InlineData("D001", 44.3, 77, -1)]
        public async Task AddTelemetry_WhenTelemetryDataIsInValidorNull_ReturnsFalse(string deviceId, decimal temperature,
            int humidity,
            int batteryLevel)
        {
            // Arrange
            Device device = CreateValidDevice();
            Telemetry telemetry = new Telemetry
            {
                DeviceID = deviceId,
                Temperature = temperature,
                Humidity = humidity,
                BatteryLevel = batteryLevel,
                RecordedAt = DateTime.Now
            };

            devicemockRepository
                .Setup(r => r.GetDeviceById(deviceId))
                .ReturnsAsync(deviceId == "D001" ? device : null);
            // Act
            bool result = await telemetryService.RecordTelemetry(telemetry);

            // Assert
            Assert.False(result);

            telemetrymockRepository.Verify(
                r => r.AddTelemetry(telemetry),
                Times.Never);
        }
        [Fact]
        public async Task GetLatestTelemetry_ReturnsLatestTelemetryData_ReturnsList()
        {
            // Arrange
            string deviceId = "D001";
            Telemetry telemetry = new Telemetry
            {
                TelemetryID = 3,
                DeviceID = "D001",
                Temperature = 44.3M,
                Humidity = 77,
                BatteryLevel = 17,
                RecordedAt = new DateTime(2026, 7, 10, 12, 30, 0)

            };


            telemetrymockRepository
                .Setup(r => r.GetLatestTelemetry(deviceId))
                .ReturnsAsync(telemetry);

            // Act
            Telemetry? result = await telemetryService.GetLatestTelemetry(deviceId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.TelemetryID);
            Assert.Equal(new DateTime(2026, 7, 10, 12, 30, 0), result.RecordedAt);
        }
        [Fact]
        public async Task GetLatestTelemetry_InvalidDeviceId_ReturnsFalse()
        {
            // Arrange
            string deviceId = "D001";
            Telemetry telemetry = new Telemetry
            {
                TelemetryID = 3,
                DeviceID = "D001",
                Temperature = 44.3M,
                Humidity = 77,
                BatteryLevel = 17,
                RecordedAt = new DateTime(2026, 7, 10, 12, 30, 0)

            };


            telemetrymockRepository
                .Setup(r => r.GetLatestTelemetry("deviceId"))
                .ReturnsAsync((Telemetry?)null);

            // Act
            Telemetry? result = await telemetryService.GetLatestTelemetry("deviceId");

            // Assert
            Assert.Null(result);
        }
        [Fact]
        public async Task GetTelemetryByydeviceId_ReturnsAllTelemetryData_ReturnsList()
        {
            // Arrange

            Device device = CreateValidDevice();
            List<Telemetry> telemetryList = new()
            {
                    new Telemetry
                    {
                        TelemetryID = 1,
                        DeviceID = "D001",
                        Temperature = 44.3M,
                        Humidity = 77,
                        BatteryLevel = 17,
                        RecordedAt = new DateTime(2026,7,10,10,0,0)
                    },
                    new Telemetry
                    {
                        TelemetryID = 2,
                        DeviceID = "D001",
                        Temperature = 44.3M,
                        Humidity = 77,
                        BatteryLevel = 17,
                        RecordedAt = new DateTime(2026,7,10,11,0,0)
                    },
                    new Telemetry
                    {
                        TelemetryID = 3,
                        DeviceID = "D001",
                        Temperature = 44.3M,
                        Humidity = 77,
                        BatteryLevel = 17,
                        RecordedAt = new DateTime(2026,7,10,12,0,0)
                    }
                };

            devicemockRepository
                .Setup(r => r.GetDeviceById(device.DeviceID))
                .ReturnsAsync(device);

            telemetrymockRepository
                .Setup(r => r.GetTelemetryByDeviceId(device.DeviceID))
                .ReturnsAsync(telemetryList);


            // Act
            List<Telemetry> result = await telemetryService.GetTelemetryHistory(device.DeviceID);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.All(result, t => Assert.Equal("D001", t.DeviceID));
        }
        [Fact]
        public async Task GetTelemetryByydeviceId_DeviceHasNoTelemetryData_ReturnsNullList()
        {

            Device device = CreateValidDevice();
            List<Telemetry> telemetryList = new List<Telemetry>();

            devicemockRepository
                .Setup(r => r.GetDeviceById(device.DeviceID))
                .ReturnsAsync(device);

            telemetrymockRepository
                .Setup(r => r.GetTelemetryByDeviceId(device.DeviceID))
                .ReturnsAsync(telemetryList);

            List<Telemetry> result = await telemetryService.GetTelemetryHistory(device.DeviceID);

            Assert.Empty(result);

        }
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public async Task GetTelemetryByydeviceId_WhenDeviceIdIsInvalid_ReturnsEmptyList(string deviceId)
        {
            List<Telemetry> result = await telemetryService.GetTelemetryHistory(deviceId);

            Assert.Empty(result);

            devicemockRepository.Verify(
                r => r.GetDeviceById(It.IsAny<string>()),
                Times.Never);

            telemetrymockRepository.Verify(
                r => r.GetTelemetryByDeviceId(It.IsAny<string>()),
                Times.Never);
        }
        [Fact]
        public async Task GetAllTelemetry_ShouldReturnAllTelemetry_WhenTelemetryExists()
        {
            List<Telemetry> telemetryList = new()
            {
                    new Telemetry
                    {
                        TelemetryID = 1,
                        DeviceID = "D001",
                        Temperature = 44.3M,
                        Humidity = 77,
                        BatteryLevel = 17,
                        RecordedAt = new DateTime(2026,7,10,10,0,0)
                    },
                    new Telemetry
                    {
                        TelemetryID = 2,
                        DeviceID = "D001",
                        Temperature = 44.3M,
                        Humidity = 77,
                        BatteryLevel = 17,
                        RecordedAt = new DateTime(2026,7,10,11,0,0)
                    },
                    new Telemetry
                    {
                        TelemetryID = 3,
                        DeviceID = "D002",
                        Temperature = 44.3M,
                        Humidity = 77,
                        BatteryLevel = 17,
                        RecordedAt = new DateTime(2026,7,10,12,0,0)
                    }
                };
            telemetrymockRepository
                .Setup(r => r.GetAllTelemetry())
                .ReturnsAsync(telemetryList);

            List<Telemetry> result = await telemetryService.GetAllTelemetry();
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
        }
        [Fact]
        public async Task GetAllDevice_ShouldReturnAllDevice_WhenDeviceDoesNotExists()
        {
            telemetrymockRepository
                .Setup(r => r.GetAllTelemetry())
                .ReturnsAsync(new List<Telemetry>());

            List<Telemetry> result = await telemetryService.GetAllTelemetry();

            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}