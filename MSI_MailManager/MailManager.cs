using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Mail;
using MSI_MailManager.Models;
using System.Reflection;
using System.Text;
using System.Collections.Generic;

namespace MSI_MailManager
{
    public class MailManager
    {
        //TODO:
        //1. Check if e-mail is valid
        //2. Send e-mail

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
                    var toAddress = new MailAddress(email.RecipientInformation.ToEmail, email.RecipientInformation.ToName);
                    string fromPassword = email.SenderInformation.FromPassword;
                    string subject = email.MessageInformation.Subject;
                    string body = email.MessageInformation.Body;

                    SmtpClient smtp = new SmtpClient
                    {
                        Host = email.SMTPInformation.Host,
                        Port = email.SMTPInformation.Port,
                        EnableSsl = email.SMTPInformation.EnableSSL,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = email.SMTPInformation.UseDefaultCredentials,
                        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                    };
                    using (var message = new MailMessage(fromAddress, toAddress)
                    {
                        Subject = subject,
                        Body = body
                    })
                    {
                        smtp.Send(message);
                        errorCollection.Append("Message has been sent.");
                        Console.WriteLine("Message has been sent.");
                    }
                }
                else
                {
                    //Not all required fields have bee supplied by the user. Let the client know.
                    Console.WriteLine(errorCollection);
                }
            }
            catch (Exception ex)
            {
                errorCollection.Append(ex.ToString());
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

                    PropertyInfo[] properties = propertyInformation.GetProperties();
                    foreach (PropertyInfo property in properties)
                    {
                        object propertyValue = property.GetValue(propertyContainer);
                        if (property.Name.ToUpper() == "PORT")
                        {
                            if ((int)propertyValue == 0)
                            {
                                fieldsWithValidationError.Add(property.Name);
                            }
                        }
                        if (propertyValue == null)
                        {
                            fieldsWithValidationError.Add(property.Name);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                errorCollection.AppendLine(ex.ToString());
            }
            if(fieldsWithValidationError.Count > 0)
            {
                if (fieldsWithValidationError.Count > 1)
                    errorCollection.Append("The following fields are required: " + string.Join(",", fieldsWithValidationError) + ".");
                else
                    errorCollection.Append("This field is required: " + fieldsWithValidationError[0] + ".");
            }
            else
            {
                result = true;
            }
            return result;
        }
    }
}
