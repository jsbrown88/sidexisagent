using System;
using System.Xml;
using System.IO;
using src.Models;

namespace AutomatedAgent.Services
{
    public class XmlParserService
    {
        // Method to parse XML and extract patient data
        public Patient Parse(string filePath)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filePath);

                // Check if the XML file contains the <PATIENT> tag
                XmlNode patientNode = xmlDoc.SelectSingleNode("//PATIENT");

                if (patientNode != null)
                {
                    Patient patient = new Patient();

                    // Extract patient data from XML
                    patient.Treat = patientNode.Attributes["treat"].Value;
                    patient.FirstName = patientNode.Attributes["firstname"].Value;
                    patient.InternalId = patientNode.Attributes["internalid"].Value;
                    patient.ExternalId = patientNode.Attributes["externalid"].Value;
                    patient.DbId = patientNode.Attributes["dbid"].Value;
                    patient.LastName = patientNode.Attributes["lastname"].Value;
                    patient.DateOfBirth = DateTime.Parse(patientNode.Attributes["dateofbirth"].Value);
                    patient.Id = patientNode.Attributes["id"].Value;
                    patient.ShowPatientData = patientNode.Attributes["showpatientdata"].Value;

                    return patient;
                }
                else
                {
                    throw new Exception("No <PATIENT> tag found in XML file.");
                }
            }
            catch (Exception ex)
            {
                // Log the error and rethrow the exception
                LoggingService.Log("ERROR", "Error parsing XML file: " + ex.Message);
                throw;
            }
        }
    }
}