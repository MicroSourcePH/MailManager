using System;
using System.Collections.Generic;

namespace MSI_MailManager.Models
{
    public class Email
    {
        public Email()
        {
            //This will prevent the properties from being NULL
            RecipientInformation = new RecipientInformation();
            SenderInformation = new SenderInformation();
            MessageInformation = new MessageInformation();
            SMTPInformation = new SMTPInformation();
        }
        public RecipientInformation RecipientInformation { get; set; }
        public SenderInformation SenderInformation { get; set; }
        public MessageInformation MessageInformation { get; set; }
        public SMTPInformation SMTPInformation { get; set; }
    }

    /// <summary>
    /// This will hold information where the e-mail will be sent to
    /// </summary>
    public class RecipientInformation
    {
        public string ToEmail { get; set; }
        public string ToName { get; set; }
    }

    /// <summary>
    /// This will be used to hold the sender's information
    /// </summary>
    public class SenderInformation
    {
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public string FromPassword { get; set; }
    }
    /// <summary>
    /// This will contain other properties of the message
    /// </summary>
    public class MessageInformation
    {
        public string Body { get; set; }
        public string Subject { get; set; }
        public bool IsHTMLBody { get; set; }
        public List<string> Attachments { get; set; }
        public bool CompressAttachments { get; set; }
        public string CompressedAttachmentFileName { get; set;  }
    }

    /// <summary>
    /// This will contain the STMP settings provided by the client
    /// </summary>
    public class SMTPInformation
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSSL { get; set; }
        public bool UseDefaultCredentials { get; set; }

    }
}
