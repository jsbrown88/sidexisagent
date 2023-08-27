namespace src.Models
{
    public static class ApiEndpoints
    {
        // Endpoint for creating a new patient entry in the AI X-ray viewer system
        public const string PatientCreation = "https://aiv2.craniocatch.com/api/v1.8/create/patient/";

        // Endpoint for uploading the X-ray image to the AI X-ray viewer system
        public const string ImageUpload = "https://aiv2.craniocatch.com/api/v1.8/analyze/radiography/";
    }
}