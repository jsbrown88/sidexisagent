using System;
using NUnit.Framework;
using Moq;
using src.Services;

namespace src.Tests
{
    [TestFixture]
    public class ErrorHandlingServiceTests
    {
        private ErrorHandlingService _errorHandlingService;
        private Mock<LoggingService> _mockLoggingService;

        [SetUp]
        public void SetUp()
        {
            _mockLoggingService = new Mock<LoggingService>();
            _errorHandlingService = new ErrorHandlingService(_mockLoggingService.Object);
        }

        [Test]
        public void Test_HandleError_LogsError()
        {
            // Arrange
            var exception = new Exception("Test exception");

            // Act
            _errorHandlingService.HandleError(exception);

            // Assert
            _mockLoggingService.Verify(x => x.LogError(It.IsAny<string>(), exception), Times.Once);
        }

        [Test]
        public void Test_RetryOperation_RetriesOnFailure()
        {
            // Arrange
            var retryCount = 0;
            Action operation = () =>
            {
                retryCount++;
                if (retryCount < 3)
                {
                    throw new Exception("Test exception");
                }
            };

            // Act
            _errorHandlingService.RetryOperation(operation, 3);

            // Assert
            Assert.AreEqual(3, retryCount);
        }

        [Test]
        public void Test_RetryOperation_DoesNotRetryOnSuccess()
        {
            // Arrange
            var retryCount = 0;
            Action operation = () => { retryCount++; };

            // Act
            _errorHandlingService.RetryOperation(operation, 3);

            // Assert
            Assert.AreEqual(1, retryCount);
        }
    }
}