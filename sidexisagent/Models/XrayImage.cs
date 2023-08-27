using System;

namespace src.Models
{
    public class XrayImage
    {
        // The path of the X-ray image file
        public string FilePath { get; set; }

        // The name of the X-ray image file
        public string FileName { get; set; }

        // The format of the X-ray image file
        public string Format { get; set; }

        // The patient ID associated with the X-ray image
        public string PatientId { get; set; }

        // The date and time the X-ray image was created
        public DateTime CreatedAt { get; set; }
    }
}