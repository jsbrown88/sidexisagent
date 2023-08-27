using System;
using System.Net.Http;
using System.Threading.Tasks;
using src.Models;

namespace AutomatedAgent.Services
{
    public class PatientCreationService
    {
        private readonly HttpClient _httpClient;
        private readonly LoggingService _loggingService;

        public PatientCreationService(HttpClient httpClient, LoggingService loggingService)
        {
            _httpClient = httpClient;
            _loggingService = loggingService;
        }

        // Method to create a new patient entry in the AI X-ray viewer system
        public async Task<bool> CreatePatient(Patient patient)
        {
            try
            {
                // API endpoint for patient creation
                string patientCreationEndpoint = ApiEndpoints.PatientCreation;

                // Send a POST request to the patient creation endpoint
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync(patientCreationEndpoint, patient);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    _loggingService.LogInfo($"Successfully created patient with ID {patient.Id}");
                    return true;
                }
                else
                {
                    _loggingService.LogError($"Failed to create patient with ID {patient.Id}. Status code: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Log the exception and continue
                _loggingService.LogError($"Exception occurred while creating patient with ID {patient.Id}: {ex.Message}");
                return false;
            }
        }
    }
}