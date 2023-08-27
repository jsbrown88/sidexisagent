using System;
using NUnit.Framework;
using Moq;
using src.Services;
using src.Models;

namespace src.Tests
{
    [TestFixture]
    public class PatientCreationServiceTests
    {
        private Mock<HttpClient> _httpClientMock;
        private PatientCreationService _patientCreationService;

        [SetUp]
        public void SetUp()
        {
            _httpClientMock = new Mock<HttpClient>();
            _patientCreationService = new PatientCreationService(_httpClientMock.Object);
        }

        [Test]
        public void CreatePatient_ValidPatientData_ReturnsTrue()
        {
            // Arrange
            var patient = new Patient
            {
                Treat = "Dr. Demo",
                FirstName = "Mohammad",
                LastName = "Homoud",
                InternalId = "9887",
                ExternalId = "13484",
                DbId = "00000000000000",
                DateOfBirth = new DateTime(2023, 04, 13),
                Id = "9887",
                ShowPatientData = "$PatientID$ - $LastName$, $FirstName$ *$Birthdate_short$"
            };

            _httpClientMock.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

            // Act
            var result = _patientCreationService.CreatePatient(patient);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void CreatePatient_InvalidPatientData_ReturnsFalse()
        {
            // Arrange
            var patient = new Patient();

            _httpClientMock.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.BadRequest));

            // Act
            var result = _patientCreationService.CreatePatient(patient);

            // Assert
            Assert.IsFalse(result);
        }
    }
}