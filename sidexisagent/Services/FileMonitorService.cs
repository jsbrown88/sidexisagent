using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Configuration;

namespace AutomatedAgent.Services
{
    public class FileMonitorService
    {
        public event EventHandler<string> OnNewXmlFileDetected;
        private readonly string _path;
        private readonly FileSystemWatcher _watcher;
        private readonly XmlParserService _xmlParserService;
        private readonly PatientCreationService _patientCreationService;
        private readonly ImageUploadService _imageUploadService;
        private readonly LoggingService _loggingService;

        public FileMonitorService(string path, XmlParserService xmlParserService, PatientCreationService patientCreationService, ImageUploadService imageUploadService, LoggingService loggingService)
        {
            _path = path;
            _xmlParserService = xmlParserService;
            _patientCreationService = patientCreationService;
            _imageUploadService = imageUploadService;
            _loggingService = loggingService;

            _watcher = new FileSystemWatcher
            {
                Path = _path,
                NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                Filter = "*.xml"
            };

            _watcher.Created += OnCreated;
        }

        public void StartMonitoring()
        {
            _watcher.EnableRaisingEvents = true;
        }

        private async void OnCreated(object source, FileSystemEventArgs e)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(e.FullPath);

                if (doc.DocumentElement.Name == "PATIENT")
                {
                    var patientData = await _xmlParserService.Parse(doc);
                    var patientCreated = await _patientCreationService.CreatePatient(patientData);

                    if (patientCreated)
                    {
                        var imageFilePath = GetAssociatedImage(e.FullPath);

                        // Create the XrayImage object
                        var image = new src.Models.XrayImage
                        {
                            FilePath = imageFilePath,
                            FileName = Path.GetFileName(imageFilePath),
                            Format = "tiff"
                        };

                        // Fetch the apiKey from your configuration
                        var apiKey = ConfigurationManager.AppSettings["ApiKey"];

                        await _imageUploadService.UploadImageAsync(image, apiKey);
                    }
                }

                // Raise the event after processing the XML
                OnNewXmlFileDetected?.Invoke(this, e.FullPath);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error in OnCreated: {ex.Message}");
            }
        }

        public string GetAssociatedImage(string xmlFilePath)
        {
            // Navigate up two directories to the '2D' directory
            var twoDDirectory = Directory.GetParent(Directory.GetParent(xmlFilePath).FullName).FullName;

            // Ensure that the directory ends with "2D"
            if (!twoDDirectory.EndsWith("2D")) return null;

            // Look for a sub-directory ending with 'raw'
            var rawDirectory = Directory.GetDirectories(twoDDirectory, "*_raw").FirstOrDefault();
            if (rawDirectory == null) return null;

            // Get the base filename of the XML without extension
            var baseXmlFilename = Path.GetFileNameWithoutExtension(xmlFilePath);

            // Construct the expected image path
            var expectedImagePath = Path.Combine(rawDirectory, $"{baseXmlFilename}.tiff");

            return File.Exists(expectedImagePath) ? expectedImagePath : null;
        }
    }
}
