using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SAF_OpticalFailureDetector.messenger
{
    class Messenger
    {
        private const String HOST_EMAIL = "SAF.testingemail@gmail.com";
        private const String HOST_PASS = "saf_hollan";

        private String email;
        private String testNumber;
        private String sampleNumber;
        private int cycleCount;

        public Messenger(string email, string testNumber, string sampleNumber)
        {
            this.email = email;
            this.testNumber = testNumber;
            this.sampleNumber = sampleNumber;
            this.cycleCount = 0;
        }

        public String GetTestNumber()
        {
            return testNumber;
        }

        public String GetSampleNumber()
        {
            return sampleNumber;
        }

        public String GetEmailAddress()
        {
            return email;
        }

        public int GetCycleCount()
        {
            return cycleCount;
        }

        public void SetCycleCount(int cycleCount)
        {
            this.cycleCount = cycleCount;
        }

        public void SetEmailAddress(String email)
        {
            this.email = email;
        }

        public void SetSampleNumber(String sampleNumber)
        {
            this.sampleNumber = sampleNumber;
        }
        
        public void SetTestNumber(String testNumber)
        {
            this.testNumber = testNumber;
        }

        public void SendShutdownMessage()
        {
            // setup the body of the message
            String body = "Hello," + Environment.NewLine + Environment.NewLine +
                "Error, a crack was detected in sample part." + Environment.NewLine + 
                "Test number: " + testNumber + Environment.NewLine +
                "Sample Number: " + sampleNumber + Environment.NewLine +
                "Date: " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + Environment.NewLine +
                "Cycle Count: " + cycleCount.ToString() + Environment.NewLine + Environment.NewLine +
                "Thank you," + Environment.NewLine + 
                "SAF Holland";
            String subject = "SAF Holland, Error, Test rig detected crack.";
            try
            {
                sendMessage(subject, body);
            }
            catch (Exception ex)
            {

            }
            
            
        }
        private void sendMessage(String subject, String message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(HOST_EMAIL, HOST_PASS),
                EnableSsl = true
            };
            client.Send(HOST_EMAIL, email, subject, message);
        }
    }
}
