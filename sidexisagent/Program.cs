using System;
using System.Threading.Tasks;
using AutomatedAgent.Services;
using System.Net.Http;
using System.Configuration; // Add this line to access ConfigurationManager
using src.Models;
using System.IO;

namespace AutomatedAgent
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Initialize services
                var httpClient = new HttpClient();
                var xmlParserService = new XmlParserService();
                var loggingService = new LoggingService();
                var patientCreationService = new PatientCreationService(httpClient, loggingService);
                var imageUploadService = new ImageUploadService(httpClient, loggingService);
                var errorHandlingService = new ErrorHandlingService();

                // Use the directory path from App.config
                string directoryToMonitor = ConfigurationManager.AppSettings["MonitorDirectory"];

                var fileMonitorService = new FileMonitorService(directoryToMonitor, xmlParserService, patientCreationService, imageUploadService, loggingService);

                // Start monitoring the directory
                fileMonitorService.StartMonitoring();

                // Event handler for new XML file detection
                fileMonitorService.OnNewXmlFileDetected += async (sender, filePath) =>
                {
                    try
                    {
                        // Parse XML file to extract patient data
                        var patientData = xmlParserService.Parse(filePath);

                        // Create a new patient entry in the AI X-ray viewer system
                        var patientCreated = await patientCreationService.CreatePatient(patientData);

                        if (patientCreated)
                        {
                            // Locate the X-ray image associated with the XML file
                            var imagePath = fileMonitorService.GetAssociatedImage(filePath);

                            // Create an XrayImage object
                            var xrayImage = new XrayImage
                            {
                                FilePath = imagePath,
                                FileName = Path.GetFileName(imagePath)
                            };

                            // Retrieve API key from App.config
                            string ApiKey = ConfigurationManager.AppSettings["ApiKey"];

                            // Upload the X-ray image to the AI X-ray viewer system
                            await imageUploadService.UploadImage(xrayImage, ApiKey);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle and log any errors
                        errorHandlingService.HandleException(ex);
                        loggingService.LogError(ex.Message);
                    }
                };

                // Keep the program running
                Task.Delay(-1).Wait();
            }
            catch (Exception ex)
            {
                // Log any unhandled exceptions
                var loggingService = new LoggingService();
                loggingService.LogError(ex.Message);
            }
        }
    }
}
