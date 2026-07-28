using IOTDeviceManagementSystem.Models;
using IOTDeviceManagementSystem.Repositories;
using IOTDeviceManagementSystem.Services;
using IOTDeviceManagementSystem.Interfaces;
using Moq;
using Xunit;

namespace UnitTesting
{
    public class DeviceServiceTests
    {
        private readonly Mock<IDeviceRepository> mockRepository;
        private readonly DeviceService deviceService;

        public DeviceServiceTests()
        {
            mockRepository = new Mock<IDeviceRepository>();
            deviceService = new DeviceService(mockRepository.Object);
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
        public async Task RegisterDevice_ShouldReturnTrue_WhenDeviceIsValid()
        {
            // Arrange
            Device device = CreateValidDevice();

            mockRepository
                .Setup(r => r.GetDeviceById(device.DeviceID))
                .ReturnsAsync((Device?)null);

            mockRepository
                .Setup(r => r.AddDevice(device))
                .ReturnsAsync(true);

            // Act
            bool result = await deviceService.RegisterDeviceAsync(device);

            // Assert
            Assert.True(result);

            mockRepository.Verify(
                r => r.AddDevice(device),
                Times.Once);
        }
        //When Device Already exists
        [Fact]
        public async Task RegisterDevice_ShouldReturnFalse_WhenDeviceAlreadyExists()
        {
            // Arrange
            Device device = CreateValidDevice();

            mockRepository
                .Setup(r => r.GetDeviceById(device.DeviceID))
                .ReturnsAsync(device);

            // Act
            bool result = await deviceService.RegisterDeviceAsync(device);

            // Assert
            Assert.False(result);

            mockRepository.Verify(
                r => r.AddDevice(It.IsAny<Device>()),
                Times.Never);
        }
        //When Field are missing
        [Theory]
        [InlineData("", "Temperature Sensor", "Gateway", "Lab")]
        [InlineData("D001", "", "Gateway", "Lab")]
        [InlineData("D001", "Temperature Sensor", "", "Lab")]
        [InlineData("D001", "Temperature Sensor", "Gateway", "")]
        public async Task RegisterDevice_ShouldReturnFalse_WhenRequiredFieldIsMissing(
            string id,
            string name,
            string type,
            string location)
        {
            // Arrange
            Device device = new Device
            {
                DeviceID = id,
                DeviceName = name,
                DeviceType = type,
                Location = location,
                Status = "Active",
                CreatedDate = DateTime.Now
            };

            // Act
            bool result = await deviceService.RegisterDeviceAsync(device);

            // Assert
            Assert.False(result);

            mockRepository.Verify(
                r => r.AddDevice(It.IsAny<Device>()),
                Times.Never);
        }
        //When Status is Invalid
        [Fact]
        public async Task RegisterDevice_ShouldReturnFalse_WhenStatusIsInvalid()
        {
            // Arrange
            Device device = CreateValidDevice();
            device.Status = "Running";

            // Act
            bool result = await deviceService.RegisterDeviceAsync(device);

            // Assert
            Assert.False(result);

            mockRepository.Verify(
                r => r.AddDevice(It.IsAny<Device>()),
                Times.Never);
        }
        //When Add Fails
        [Fact]
        public async Task RegisterDevice_ShouldReturnFalse_WhenRepositoryFailsToAdd()
        {
            // Arrange
            Device device = CreateValidDevice();

            mockRepository
                .Setup(r => r.GetDeviceById(device.DeviceID))
                .ReturnsAsync((Device?)null);

            mockRepository
                .Setup(r => r.AddDevice(device))
                .ReturnsAsync(false);

            // Act
            bool result = await deviceService.RegisterDeviceAsync(device);

            // Assert
            Assert.False(result);

            mockRepository.Verify(
                r => r.AddDevice(device),
                Times.Once);
        }
        //When Device data is null
        [Fact]
        public async Task RegisterDevice_ShouldReturnFalse_WhenDeviceIsNull()
        {
            // Arrange
            Device? device = null;

            // Act
            bool result = await deviceService.RegisterDeviceAsync(device);

            // Assert
            Assert.False(result);

            mockRepository.Verify(
                r => r.AddDevice(It.IsAny<Device>()),
                Times.Never);
        }
        //Invalid Device Type
        [Fact]
        public async Task RegisterDevice_ShouldReturnFalse_WhenDeviceTypeIsInvalid()
        {
            // Arrange
            Device device = CreateValidDevice();
            device.DeviceType = "Sensor";

            // Act
            bool result = await deviceService.RegisterDeviceAsync(device);

            // Assert
            Assert.False(result);

            mockRepository.Verify(
                r => r.AddDevice(It.IsAny<Device>()),
                Times.Never);
        }

        [Fact]
        public async Task GetDeviceByID_ShouldReturnTrue_When_Device_Exists()
        {
            Device device = CreateValidDevice();

            mockRepository
                .Setup(r => r.GetDeviceById(device.DeviceID))
                .ReturnsAsync(device);

            Device? result = await deviceService.GetDeviceById(device.DeviceID);

            Assert.NotNull(result);
            Assert.Equal(device.DeviceID, result.DeviceID);
            Assert.Equal(device.DeviceName, result.DeviceName);
        }

        [Fact]
        public async Task GetDeviceByID_ShouldReturnFalse_When_Device_Does_not_Exists()
        {
            mockRepository
                .Setup(r => r.GetDeviceById("sampleID"))
                .ReturnsAsync((Device?)null);

            Device? result = await deviceService.GetDeviceById("sampleID");

            Assert.Null(result);
        }
        [Fact]
        public async Task GetAllDevice_ShouldReturnAllDevice_WhenDeviceExists()
        {
            List<Device> devices = new List<Device>
        {
          CreateValidDevice(),
        new Device
        {
            DeviceID = "D002",
            DeviceName = "Telemetry Module",
            DeviceType = "TelemetryModule",
            Location = "Office",
            Status = "Active",
            CreatedDate = DateTime.Now
        }
    };
            mockRepository
                .Setup(r => r.RetrieveAllDevices())
                .ReturnsAsync(devices);

            List<Device> result = await deviceService.GetAllDevices();
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("D001", result[0].DeviceID);
            Assert.Equal("D002", result[1].DeviceID);
        }
        [Fact]
        public async Task GetAllDevice_ShouldReturnAllDevice_WhenDeviceDoesNotExists()
        {
            mockRepository
                .Setup(r => r.RetrieveAllDevices())
                .ReturnsAsync(new List<Device>());

            List<Device> result = await deviceService.GetAllDevices();

            Assert.NotNull(result);
            Assert.Empty(result);
        }
        [Fact]
        public async Task UpdateDevice_ShouldReturnTrue_WhenDeviceIsValid()
        {
            Device device = CreateValidDevice();

            mockRepository
                .Setup(r => r.GetDeviceById(device.DeviceID))
                .ReturnsAsync(device);

            mockRepository
                .Setup(r => r.UpdateDevice(device))
                .ReturnsAsync(true);

            bool result = await deviceService.ModifyDevice(device);

            Assert.True(result);

            mockRepository.Verify(
                r => r.UpdateDevice(device),
                Times.Once);
        }
        [Fact]
        public async Task UpdatDevice_ShouldReturnFalse_WhenDeviceDoesNotExists()
        {
            mockRepository
                .Setup(r => r.GetDeviceById("sampleID"))
                .ReturnsAsync((Device?)null);

            Device? device = await deviceService.GetDeviceById("sampleID");
            bool result = await deviceService.ModifyDevice(device);

            Assert.False(result);
            mockRepository.Verify(
                r => r.UpdateDevice(It.IsAny<Device>()),
                Times.Never);
        }
        [Fact]
        public async Task UpdateDevice_ShouldReturnFalse_WhenRepositoryFailsToUpdate()
        {
            Device device = CreateValidDevice();

            mockRepository
                .Setup(r => r.GetDeviceById(device.DeviceID))
                .ReturnsAsync(device);
            mockRepository
                .Setup(r => r.UpdateDevice(device))
                .ReturnsAsync(false);
            bool result = await deviceService.ModifyDevice(device);

            Assert.False(result);

            mockRepository.Verify(
                r => r.UpdateDevice(device),
                Times.Once);
        }
        [Fact]
        public async Task UpdateDevice_ShouldReturnFalse_WhenDeviceIsNull()
        {
            Device? device = null;

            bool result = await deviceService.ModifyDevice(device);

            Assert.False(result);

            mockRepository.Verify(
                r => r.GetDeviceById(It.IsAny<string>()),
                Times.Never);
            mockRepository.Verify(
                r => r.UpdateDevice(It.IsAny<Device>()),
                Times.Never);
        }

        [Fact]
        public async Task DeleteDevice_ShouldReturnFalse_WhenDeviceExists()
        {
            Device device = CreateValidDevice();
            mockRepository
                .Setup(r => r.GetDeviceById(device.DeviceID))
                .ReturnsAsync(device);

            mockRepository
                .Setup(r => r.DeleteDevice(device.DeviceID))
                .ReturnsAsync(true);

            bool result = await deviceService.RemoveDevice(device.DeviceID);

            Assert.True(result);

            mockRepository.Verify(
                r => r.DeleteDevice(device.DeviceID),
                Times.Once);
        }
        [Fact]
        public async Task DeleteDevice_ShouldReturnFalse_WhenDeviceDoesNotExists()
        {
            string deviceId = "sampleID";
            mockRepository
                .Setup(r => r.GetDeviceById(deviceId))
                .ReturnsAsync((Device?)null);

            bool result = await deviceService.RemoveDevice(deviceId);

            Assert.False(result);

            mockRepository.Verify(
                r => r.DeleteDevice(It.IsAny<string>()),
                Times.Never);
        }
        [Fact]
        public async Task DeleteDevice_ShouldReturnFalse_WhenDeleteFails()
        {
            Device device = CreateValidDevice();
            mockRepository
                .Setup(r => r.GetDeviceById(device.DeviceID))
                .ReturnsAsync(device);

            mockRepository
                .Setup(r => r.DeleteDevice(device.DeviceID))
                .ReturnsAsync(false);

            bool result = await deviceService.RemoveDevice(device.DeviceID);

            Assert.False(result);

            mockRepository.Verify(
                r => r.DeleteDevice(device.DeviceID),
                Times.Once);
        }

    }
}
