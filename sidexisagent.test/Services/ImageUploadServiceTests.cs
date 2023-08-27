using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Moq;
using src.Services;

namespace src.Tests
{
    public class ImageUploadServiceTests
    {
        private readonly ImageUploadService _imageUploadService;
        private readonly Mock<HttpClient> _mockHttpClient;

        public ImageUploadServiceTests()
        {
            _mockHttpClient = new Mock<HttpClient>();
            _imageUploadService = new ImageUploadService(_mockHttpClient.Object);
        }

        [Fact]
        public async Task UploadImageAsync_ShouldReturnTrue_WhenUploadIsSuccessful()
        {
            // Arrange
            var imagePath = "/path/to/image.jpg";
            var apiEndpoint = "https://aiv2.craniocatch.com/api/v1.8/analyze/radiography/";
            var apiKey = "YOURAPIKEY";

            _mockHttpClient.Setup(client => client.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .ReturnsAsync(new HttpResponseMessage(System.Net.HttpStatusCode.OK));

            // Act
            var result = await _imageUploadService.UploadImageAsync(imagePath, apiEndpoint, apiKey);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UploadImageAsync_ShouldReturnFalse_WhenUploadFails()
        {
            // Arrange
            var imagePath = "/path/to/image.jpg";
            var apiEndpoint = "https://aiv2.craniocatch.com/api/v1.8/analyze/radiography/";
            var apiKey = "YOURAPIKEY";

            _mockHttpClient.Setup(client => client.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .ReturnsAsync(new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError));

            // Act
            var result = await _imageUploadService.UploadImageAsync(imagePath, apiEndpoint, apiKey);

            // Assert
            Assert.False(result);
        }
    }
}