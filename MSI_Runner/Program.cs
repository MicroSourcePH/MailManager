using System;

namespace MSI_Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            MSI_MailManager.MailManager mailManager  = new MSI_MailManager.MailManager();
            string result = mailManager.SendEmail(new MSI_MailManager.Models.Email()
            {
                MessageInformation = new MSI_MailManager.Models.MessageInformation()
                {
                    Body = "TEST BODY",
                    isHTMLBody = false,
                    Subject = "TEST SUBJECT"
                },
                RecipientInformation = new MSI_MailManager.Models.RecipientInformation()
                {
                    ToEmail = "hil.jacla@gmail.com",
                    ToName = "Hilario Jacla III"
                },
                SenderInformation = new MSI_MailManager.Models.SenderInformation()
                {
                    FromEmail = "offshoreconfie@gmail.com",
                    FromName = "Hilario Jacla III",
                    FromPassword = "7tfRPX-=",
                },
                SMTPInformation = new MSI_MailManager.Models.SMTPInformation()
                {
                    EnableSSL = true,
                    Host = "smtp.gmail.com",
                    Port = 587,
                    UseDefaultCredentials = true
                }
            });
            Console.WriteLine(result);
        }
    }
}
