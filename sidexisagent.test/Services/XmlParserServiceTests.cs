using System;
using System.IO;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using src.Services;

namespace src.Tests
{
    [TestClass]
    public class XmlParserServiceTests
    {
        private XmlParserServiceTests _xmlParserService;

        [TestInitialize]
        public void TestInitialize()
        {
            _xmlParserService = new XmlParserServiceTests();
        }

        [TestMethod]
        public void TestParseXmlFile()
        {
            // Arrange
            var xmlContent = "<PATIENT treat=\"Dr. Demo\" firstname=\"mohammad homoud\" internalid=\"9887\" externalid=\"13484\" dbid=\"00000000000000\" lastname=\"haddadi\" dateofbirth=\"2023-04-13\" id=\"9887\" showpatientdata=\"$PatientID$ - $LastName$, $FirstName$ *$Birthdate_short$\" />";
            var xmlFilePath = Path.GetTempFileName();
            File.WriteAllText(xmlFilePath, xmlContent);

            // Act
            var patient = _xmlParserService.ParseXmlFile(xmlFilePath);

            // Assert
            Assert.AreEqual("Dr. Demo", patient.Treat);
            Assert.AreEqual("mohammad homoud", patient.FirstName);
            Assert.AreEqual("9887", patient.InternalId);
            Assert.AreEqual("13484", patient.ExternalId);
            Assert.AreEqual("00000000000000", patient.DbId);
            Assert.AreEqual("haddadi", patient.LastName);
            Assert.AreEqual(new DateTime(2023, 4, 13), patient.DateOfBirth);
            Assert.AreEqual("9887", patient.Id);
            Assert.AreEqual("$PatientID$ - $LastName$, $FirstName$ *$Birthdate_short$", patient.ShowPatientData);

            // Cleanup
            File.Delete(xmlFilePath);
        }

        [TestMethod]
        public void TestParseXmlFile_NoPatientTag()
        {
            // Arrange
            var xmlContent = "<NOTPATIENT treat=\"Dr. Demo\" firstname=\"mohammad homoud\" internalid=\"9887\" externalid=\"13484\" dbid=\"00000000000000\" lastname=\"haddadi\" dateofbirth=\"2023-04-13\" id=\"9887\" showpatientdata=\"$PatientID$ - $LastName$, $FirstName$ *$Birthdate_short$\" />";
            var xmlFilePath = Path.GetTempFileName();
            File.WriteAllText(xmlFilePath, xmlContent);

            // Act
            var patient = _xmlParserService.ParseXmlFile(xmlFilePath);

            // Assert
            Assert.IsNull(patient);

            // Cleanup
            File.Delete(xmlFilePath);
        }
    }
}