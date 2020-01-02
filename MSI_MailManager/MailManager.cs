using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Runtime.InteropServices;
using MSI_MailManager.Models;

namespace MSI_MailManager
{
    public class MailManager
    {
        /// <summary>
        /// Checks if the provided e-mail address follows the correct format
        /// </summary>
        /// <param name="emailAddress">E-mail address string to check</param>
        /// <returns></returns>
        public bool IsEmailValid(string emailAddress)
        {
            return (new EmailAddressAttribute().IsValid(emailAddress) && new Regex(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$").IsMatch(emailAddress));
        }


        /// <summary>
        /// Sends e-mail based on the information provided by the client
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public string SendEmail(Email email)
        {
            StringBuilder errorCollection = new StringBuilder();
            try
            {
                if (AllRequiredFieldsSuppliedByTheUser(email, out errorCollection))
                {
                    var fromAddress = new MailAddress(email.SenderInformation.FromEmail, email.SenderInformation.FromName);
                    //var toAddress = new MailAddress(email.RecipientInformation.ToEmail, email.RecipientInformation.ToName);
                    string fromPassword = email.SenderInformation.FromPassword;
                    string subject = email.MessageInformation.Subject;
                    string body = email.MessageInformation.Body;

                    SmtpClient smtp = new SmtpClient
                    {
                        Host = email.SMTPInformation.Host,
                        Port = email.SMTPInformation.Port == 0 ? (email.SMTPInformation.EnableSSL ? 587 : 25) : email.SMTPInformation.Port,
                        EnableSsl = email.SMTPInformation.EnableSSL,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = email.SMTPInformation.UseDefaultCredentials,
                        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                    };
                    using var message = new MailMessage()
                    {
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = email.MessageInformation.IsHTMLBody,
                    };

                    foreach (RecipientInformation recipient in email.RecipientInformation)
                    {
                        message.To.Add(new MailAddress(recipient.ToEmail));
                    }

                    message.From = fromAddress;

                    if (email.MessageInformation.Attachments?.Count > 0)
                    {
                        if (email.MessageInformation.CompressAttachments)
                        {
                            string zipFilePathAndName = ArchiveAttachments(email.MessageInformation.Attachments, email.MessageInformation.CompressedAttachmentFileName);
                            message.Attachments.Add(new Attachment(zipFilePathAndName));
                        }
                        else
                        {
                            foreach (string attachement in email.MessageInformation.Attachments)
                            {
                                message.Attachments.Add(new Attachment(attachement));
                            }
                        }
                    }
                    smtp.Send(message);
                    errorCollection.Append("Message has been sent.");
                }
                else
                {
                    //Not all required fields have bee supplied by the user. Let the client know.
                    Console.WriteLine(errorCollection);
                }
            }
            catch (Exception ex)
            {
                errorCollection.Append(ex.Message + " ");
                errorCollection.Append("Make sure that the SMTP Host and Port is correct.");
            }
            return errorCollection.ToString();
        }

        /// <summary>
        /// This will check each properties of the e-mail class for any required values
        /// that was not supplied by the user.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="errorCollection">List of validation errors found during the nullity check.</param>
        /// <returns></returns>
        private bool AllRequiredFieldsSuppliedByTheUser(Email email, out StringBuilder errorCollection)
        {
            List<string> fieldsWithValidationError = new List<string>();
            errorCollection = new StringBuilder();
            bool result = false;
            try
            {
                for (int x = 0; x < 4; x++)
                {
                    //0 = Message Information
                    //1 = Recipient Information
                    //2 = Sender Information
                    //3 = SMTP Information
                    Type propertyInformation = null;
                    Object propertyContainer = null;
                    switch (x)
                    {
                        case 0:
                            propertyInformation = email.MessageInformation.GetType();
                            propertyContainer = email.MessageInformation;
                            break;
                        case 1:
                            propertyInformation = email.RecipientInformation.GetType();
                            propertyContainer = email.RecipientInformation;
                            break;
                        case 2:
                            propertyInformation = email.SenderInformation.GetType();
                            propertyContainer = email.SenderInformation;
                            break;
                        case 3:
                            propertyInformation = email.SMTPInformation.GetType();
                            propertyContainer = email.SMTPInformation;
                            break;
                    }

                    if (propertyContainer == email.RecipientInformation)
                    {
                        //Ensures that we have at least 1 recipient
                        if (email.RecipientInformation.Count < 1)
                            errorCollection.Append("Please specify at least 1 recipient.");
                    }
                    else
                    {
                        PropertyInfo[] properties = propertyInformation.GetProperties();
                        foreach (PropertyInfo property in properties)
                        {
                            object propertyValue = property.GetValue(propertyContainer);
                            if (propertyValue == null)
                            {
                                if (property.Name.ToUpper() != "ATTACHMENTS" && property.Name.ToUpper() != "COMPRESSEDATTACHMENTFILENAME")
                                    fieldsWithValidationError.Add(property.Name);

                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                errorCollection.AppendLine("An exception occurred while processing your request.");
            }
            if (fieldsWithValidationError.Count > 0 || errorCollection.Length != 0)
            {
                if (fieldsWithValidationError.Count > 1)
                    errorCollection.Append("The following fields are required: " + string.Join(",", fieldsWithValidationError) + ".");
                else if (fieldsWithValidationError.Count == 1)
                    errorCollection.Append("This field is required: " + fieldsWithValidationError[0] + ".");
            }
            else
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// Creates a zipped copy of the attachment files
        /// </summary>
        /// <param name="attachmentPaths">Files to be archived</param>
        /// <param name="targetZipFileName"></param>
        /// <returns></returns>
        private string ArchiveAttachments(List<string> attachmentPaths, string targetZipFileName)
        {
            targetZipFileName = EscapePath(targetZipFileName);
            string currentAssemblyPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string stagingFolderPath = currentAssemblyPath + targetZipFileName;

            if (Directory.Exists(stagingFolderPath))
            {
                //When the name specified by the client already exists, we append a 4 digit random number.
                Random rand = new Random();
                int randomNumberToAppend = rand.Next(1000, 9999);
                stagingFolderPath += "_" + randomNumberToAppend;
                targetZipFileName += "_" + randomNumberToAppend;
            }
                
            Directory.CreateDirectory(stagingFolderPath);
            foreach(string path in attachmentPaths)
            {
                string targetFilePathAndName = stagingFolderPath + EscapePath(Path.GetFileName(path));
                File.Copy(path, targetFilePathAndName);
            }
            ZipFile.CreateFromDirectory(stagingFolderPath, currentAssemblyPath + targetZipFileName + ".zip");
            return currentAssemblyPath + targetZipFileName + ".zip";

        }

        /// <summary>
        /// This ensures that the path we create is correct for what OS the program is running.
        /// Windows uses \ to separate directory and all else uses /
        /// </summary>
        /// <param name="fileOrFolder">Path or File to escape</param>
        /// <returns></returns>
        private string EscapePath(string fileOrFolder)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return "/" + fileOrFolder;
            }
            return @"\" + fileOrFolder;
        }
    }
}
