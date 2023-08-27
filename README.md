# sidexisagent

We are embarking on the development of an automated agent designed to operate on our Windows Vista-based file server. This agent's primary function is to continuously monitor a designated directory for the addition of new subfolders and XML files. Upon detecting a new XML file, the agent should extract patient data from it. Subsequently, it will communicate with our AI X-ray viewer system to create a patient entry using the extracted data. Once the patient's creation is confirmed successfully, the agent should locate the corresponding X-ray image in the same directory and upload it to the AI X-ray viewer through a specified API endpoint.

Technically, we are building this solution using C# with the .NET Framework 4.6. Given the constraints of our operating system, windows vista, this stack offers a balance of performance, compatibility, and robustness. Essential to the agent's operation are professional error handling mechanisms and comprehensive logging of all activities, be they successful operations or encountered issues. The logging will help us maintain transparency in the agent's operations and assist in any troubleshooting endeavors.

Please review the detailed functional and technical requirements provided and kickstart the development process. This agent is a critical component in streamlining our patient data handling and X-ray processing, ensuring efficiency and accuracy.

Remember, while implementing each feature, we should write comprehensive inline comments and documentation. This should cover all aspects of the feature, including its purpose, how it works, and any important technical details This will ensure that the codebase is easy to understand and maintain in the future. The AGI should also take care to build tests for each component and functionality to ensure the reliability and robustness of the application.

Functional Requirements:
1. File Monitoring:
The system must continuously monitor a predefined directory for the creation of new subfolders with random names that contain both X-ray images in .jpg .png or dicom format as well .xml files that contain metadata on the X-rays.
Upon detecting a new .xml file, the system must process it to extract patient data only if it finds the "<PATIENT>" tag, since multiple .xml files are create per X-ray but only 1 contains the <PATIENT> tag. 
Here is an example of the information we are looking for : <PATIENT treat="Dr. Demo" firstname="mohammad homoud" internalid="9887" externalid="13484" dbid="00000000000000" lastname="haddadi" dateofbirth="2023-04-13" id="9887" showpatientdata="$PatientID$ - $LastName$, $FirstName$ *$Birthdate_short$" />
2. Patient Creation:
The system must have a mechanism to create a new patient entry in the AI X-ray viewer system using the extracted data from the XML.
Each patient's creation must be confirmed before proceeding to upload the corresponding X-ray image.
X-ray Image Upload:
After successful patient creation, the system should locate the X-ray image associated with the XML file in the same directory.
3. Image uploading
The X-ray image must be uploaded to the AI X-ray viewer system using the provided API endpoint.
curl --location 'https://aiv2.craniocatch.com/api/v1.8/analyze/radiography/' \\ --form 'image=@"/path/to/file"' \\ --form 'company.api\_key="YOURAPIKEY
4. Error Handling:
The system must gracefully handle any errors that occur during file monitoring, XML parsing, patient creation, or image upload.
In the event of an error, the system must not crash but instead log the error and continue monitoring for new files.
5. Logging:
All system activities, including successful operations, warnings, and errors, must be logged.
Logs should be stored in a readable format and easily accessible for review.

Technical Requirements:
Tech Stack:
1. Language: C#
2. Framework: .NET Framework 4.6
3. Logging: NLog or log4net for structured logging in C#.

Architecture:
1. File Monitoring: Use the FileSystemWatcher class to monitor the root directory for new subfolders and files.
2. XML Parsing: Utilize the built-in XML parsing capabilities in C# to extract patient data.
3. API Interaction: Use the HttpClient class to interact with external APIs for patient creation and image upload.
API Calls Sequence:
1. Patient Creation: As soon as the XML is parsed, make a call to the (placeholder) patient creation endpoint.
2. Image Upload: After the patient creation is successful, make a call to the provided AI X-ray viewer API endpoint to upload the X-ray image.
Error Handling: Handle potential errors at every stage:
1. File detection & reading
2. XML parsing
3. API calls (both patient creation and image upload)
Exception Handling and Retry Mechanisms Requirements:
1. Identification of Critical Sections:
Identify parts of the code that interact with external systems, especially API calls, file I/O operations, and database transactions.
These sections are most susceptible to transient failures due to network issues, server overloads, or other unforeseen circumstances.
2. Granular Exception Handling:
Implement specific exception handling for known exception types (e.g., HttpRequestException, IOException).
For each specific exception type, log detailed error messages to help in troubleshooting.
3. Fallback Strategy:
Implement a fallback strategy if a particular operation fails repeatedly. For example, after x failed attempts to call an API, the system might temporarily store the data locally and try again later.
4. Retry Mechanism:
Implement a retry mechanism using exponential backoff strategy. This means that after each failed attempt, the system waits for a longer period before trying again.
Set a maximum retry count to prevent infinite loops of retries. Typically, 3 to 5 retries are reasonable.
4. External Configuration:
Allow the maximum retry count and initial delay for the exponential backoff strategy to be configurable via the App.config. This provides flexibility to adjust these parameters without modifying the code.
5. Circuit Breaker Pattern (Optional but Recommended):
Implement a circuit breaker pattern for external services/APIs. If a service fails consistently, the circuit breaker "trips" and prevents further calls to that service for a predefined period. This ensures that failing services aren't continuously hammered with requests.
Once the circuit breaker is tripped, it periodically allows a few requests to test if the service is healthy again. If these test requests succeed, the circuit breaker is reset; otherwise, it remains tripped.
Logging:
1. Log every exception with detailed information, including exception type, message, stack trace, and any other relevant data.
2. For retries, log the retry attempt number and the delay before the next attempt.
3. When the maximum retry count is reached, log a warning or error indicating that the operation has been abandoned after several attempts.
Notifications:
1. Implement a notification mechanism to alert developers or system administrators when a critical operation fails repeatedly. This can be done via email, SMS, or other alerting systems.
Unit Testing:
1. Develop unit tests to simulate exceptions and validate that the retry mechanisms work as expected.
2. Test the maximum retry count, exponential backoff delays, and any other logic related to error handling.
Documentation:
1. Document the error handling and retry mechanisms, including the reasons for chosen strategies and configurations.
2. Provide guidance on how to interpret logs related to exceptions and retries, and what actions should be taken when certain errors are encountered.
3. In case of an error, the system should log the exact nature of the error and continue with its operation.
Logging:
1. Implement a centralized logging system using NLog or log4net.
Log levels:
1. INFO: For regular activities like file detection and successful API calls.
2. WARNING: For non-critical issues that don't halt the process but might need attention.
3. ERROR: For critical issues that could impact the operation.
4. Store logs in a structured format (e.g., .log files) in a designated directory.
5. Regularly rotate and archive logs to ensure the logging directory doesn't run out of space.
