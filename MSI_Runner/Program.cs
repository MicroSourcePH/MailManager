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
            List<string> receipients = new List<string>();
            receipients.Add("hilario.jaclaiii@confiegroup.com");
            List<string> attachments = new Program().GetTestAttachments(2);
            EmailResult result = MailManager.SendEmail(new Email()
            {
                MessageInformation = new MessageInformation()
                {
                    Body = "Message body goes here",
                    Subject = "Message subject goes here",

                    //We can leave these properties unassigned
                    IsHTMLBody = false,
                    Attachments = attachments,
                    CompressAttachments = true,
                    CompressedAttachmentFileName = "Archive File Name"
                },
                Recipients = receipients,
                SenderInformation = new SenderInformation()
                {
                    FromEmail = "offshoreconfie@gmail.com",

                    //We can leave this blank
                    FromPassword = "7tfRPX-=",
                },
                SMTPInformation = new SMTPInformation()
                {
                    EnableSSL = true,
                    Host = "smtp.gmail.com",

                    //We can leave these properties unassigned
                    UseDefaultCredentials = true
                }
            }); 
            Console.WriteLine(result.ResultMessage);
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
