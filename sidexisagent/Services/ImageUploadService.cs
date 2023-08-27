using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using src.Models;
using AutomatedAgent.Services;

namespace AutomatedAgent.Services
{
    public class ImageUploadService
    {
        private readonly HttpClient _httpClient;
        private readonly LoggingService _loggingService;

        public ImageUploadService(HttpClient httpClient, LoggingService loggingService)
        {
            _httpClient = httpClient;
            _loggingService = loggingService;
        }

        // Method to upload the X-ray image to the AI X-ray viewer system
        public async Task UploadImage(XrayImage image, string apiKey)
        {
            try
            {
                // Prepare the request
                var request = new HttpRequestMessage(HttpMethod.Post, ApiEndpoints.ImageUpload);
                var content = new MultipartFormDataContent();
                content.Add(new ByteArrayContent(File.ReadAllBytes(image.FilePath)), "image", image.FileName);
                content.Add(new StringContent(apiKey), "company.api_key");
                request.Content = content;

                // Send the request and get the response
                var response = await _httpClient.SendAsync(request);

                // Check the response
                if (response.IsSuccessStatusCode)
                {
                    _loggingService.LogInfo($"Image {image.FileName} uploaded successfully.");
                }
                else
                {
                    _loggingService.LogError($"Failed to upload image {image.FileName}. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Exception occurred while uploading image {image.FileName}: {ex.Message}");
            }
        }
    }
}