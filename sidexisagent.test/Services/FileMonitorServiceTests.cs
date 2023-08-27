using System;
using System.IO;
using NUnit.Framework;
using Moq;
using src.Services;

namespace src.Tests
{
    [TestFixture]
    public class FileMonitorServiceTests
    {
        private Mock<FileSystemWatcher> _fileSystemWatcherMock;
        private FileMonitorService _fileMonitorService;

        [SetUp]
        public void SetUp()
        {
            _fileSystemWatcherMock = new Mock<FileSystemWatcher>();
            _fileMonitorService = new FileMonitorService(_fileSystemWatcherMock.Object);
        }

        [Test]
        public void ShouldStartMonitoringWhenDirectoryExists()
        {
            // Arrange
            var directoryPath = @"C:\TestDirectory";
            Directory.CreateDirectory(directoryPath);
            _fileSystemWatcherMock.Setup(f => f.Path).Returns(directoryPath);

            // Act
            _fileMonitorService.StartMonitoring(directoryPath);

            // Assert
            _fileSystemWatcherMock.VerifySet(f => f.EnableRaisingEvents = true);
        }

        [Test]
        public void ShouldThrowExceptionWhenDirectoryDoesNotExist()
        {
            // Arrange
            var directoryPath = @"C:\NonExistentDirectory";

            // Act & Assert
            Assert.Throws<DirectoryNotFoundException>(() => _fileMonitorService.StartMonitoring(directoryPath));
        }

        [Test]
        public void ShouldStopMonitoringWhenCalled()
        {
            // Arrange
            var directoryPath = @"C:\TestDirectory";
            Directory.CreateDirectory(directoryPath);
            _fileSystemWatcherMock.Setup(f => f.Path).Returns(directoryPath);
            _fileMonitorService.StartMonitoring(directoryPath);

            // Act
            _fileMonitorService.StopMonitoring();

            // Assert
            _fileSystemWatcherMock.VerifySet(f => f.EnableRaisingEvents = false);
        }
    }
}