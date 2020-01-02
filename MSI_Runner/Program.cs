using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using MSI_MailManager;
using MSI_MailManager.Models;

namespace MSI_Runner
{
    public class Program
    {
        static void Main(string[] args)
        {
            MailManager mailManager = new MailManager();
            List<RecipientInformation> receipients = new List<RecipientInformation>
            {
                //We put this in a class for now as we might add other properties in future releases.
                new RecipientInformation() { ToEmail = "hil.jacla@gmail.com" }
            };
            //Add other recipients

            List<string> attachments = new List<string>();
            attachments.Add("/users/hiljaclaiii/testAttachment.txt");
            //Add other attachments

            string result = mailManager.SendEmail(new Email()
            {
                MessageInformation = new MessageInformation()
                {
                    Body = "TEST BODY",
                    IsHTMLBody = false,
                    Subject = "TEST SUBJECT",
                    Attachments = attachments,
                    CompressAttachments = true,
                    CompressedAttachmentFileName = "TESTARCHIVE"
                },
                RecipientInformation = receipients,
                SenderInformation = new SenderInformation()
                {
                    FromEmail = "offshoreconfie@gmail.com",
                    FromName = "Hilario Jacla III",
                    FromPassword = "",
                },
                SMTPInformation = new SMTPInformation()
                {
                    EnableSSL = true,
                    Host = "smtp.gmail.com",
                    Port = 587,
                    UseDefaultCredentials = true
                }
            });
            Console.WriteLine(result);
        }

        /// <summary>
        /// Generates a text file in the current assembly directory and sends the path to the caller
        /// </summary>
        /// <returns></returns>
        public List<string> GetTestAttachments(int fileCount)
        {
            List<string> filePaths = new List<string>();
            for (int x = 0; x < fileCount; x++)
            {
                string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/SamleTextFile_" + x + ".txt";
                try
                {
                    if (!File.Exists(path))
                    {
                        File.Create(path).Close();
                        TextWriter tw = new StreamWriter(path);
                        tw.WriteLine("This is an auto-generated text.");
                        tw.Close();
                    }
                    else if (File.Exists(path))
                    {
                        TextWriter tw = new StreamWriter(path);
                        tw.WriteLine("This is an auto-generated text.");
                        tw.Close();
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e);
                }
                filePaths.Add(path);
            }
            return filePaths;
        }
    }
}
