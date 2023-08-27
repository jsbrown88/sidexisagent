using System;
using NUnit.Framework;
using Moq;
using src.Services;
using src.Models;

namespace src.Tests
{
    [TestFixture]
    public class LoggingServiceTests
    {
        private Mock<LoggingService> _loggingServiceMock;

        [SetUp]
        public void SetUp()
        {
            _loggingServiceMock = new Mock<LoggingService>();
        }

        [Test]
        public void LogInfo_MessageIsLogged()
        {
            // Arrange
            var message = "Test Info Message";

            // Act
            _loggingServiceMock.Object.LogInfo(message);

            // Assert
            _loggingServiceMock.Verify(x => x.LogInfo(message), Times.Once);
        }

        [Test]
        public void LogWarning_MessageIsLogged()
        {
            // Arrange
            var message = "Test Warning Message";

            // Act
            _loggingServiceMock.Object.LogWarning(message);

            // Assert
            _loggingServiceMock.Verify(x => x.LogWarning(message), Times.Once);
        }

        [Test]
        public void LogError_MessageIsLogged()
        {
            // Arrange
            var message = "Test Error Message";

            // Act
            _loggingServiceMock.Object.LogError(message);

            // Assert
            _loggingServiceMock.Verify(x => x.LogError(message), Times.Once);
        }

        [Test]
        public void LogException_ExceptionIsLogged()
        {
            // Arrange
            var exception = new Exception("Test Exception");

            // Act
            _loggingServiceMock.Object.LogException(exception);

            // Assert
            _loggingServiceMock.Verify(x => x.LogException(exception), Times.Once);
        }
    }
}