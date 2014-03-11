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
        private string sendEmail;
        private string testNumber;
        private string sampleNumber;
        private int cycleCount = 5;

            // testnumber
            // sample number
            // timestamp
        public Messenger(string email, string testNumber, string sampleNumber)
        {
            sendEmail = email;
            this.testNumber = testNumber;
            this.sampleNumber = sampleNumber;
        }
        public void SendMessage()
        {

            String body = "Hello," + Environment.NewLine + Environment.NewLine +
                "Test number: " + testNumber + Environment.NewLine +
                "Sample Number: " + sampleNumber + Environment.NewLine +
                "Date: " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + Environment.NewLine +
                "Cycle Count: " + cycleCount.ToString() + Environment.NewLine + Environment.NewLine +
                "Thank you," + Environment.NewLine + 
                "SAF Holland";
            var client = new SmtpClient("smtp.gmail.com",587)
            {
                Credentials = new NetworkCredential("SAF.testingemail@gmail.com","safhollan"),
                EnableSsl = true
                
            };
            client.Send("saf.testingemail@gmail.com",sendEmail,"Testing RIG Failure",body);
            
        }
    }
}
