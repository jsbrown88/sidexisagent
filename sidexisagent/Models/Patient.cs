using System;

namespace src.Models
{
    public class Patient
    {
        public string Treat { get; set; }
        public string FirstName { get; set; }
        public string InternalId { get; set; }
        public string ExternalId { get; set; }
        public string DbId { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Id { get; set; }
        public string ShowPatientData { get; set; }
    }
}