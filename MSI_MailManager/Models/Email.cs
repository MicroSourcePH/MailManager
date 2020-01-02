using System.Collections.Generic;

namespace MSI_MailManager.Models
{
    public class Email
    {
        public Email()
        {
            //This will prevent the properties from being NULL
            RecipientInformation = new List<RecipientInformation>();
            SenderInformation = new SenderInformation();
            MessageInformation = new MessageInformation();
            SMTPInformation = new SMTPInformation();
        }
        /// <summary>
        /// Container for recipient related information.
        /// </summary>
        public List<RecipientInformation> RecipientInformation { get; set; }

        /// <summary>
        /// Container for sender related information.
        /// </summary>
        public SenderInformation SenderInformation { get; set; }

        /// <summary>
        /// Container for message related information.
        /// </summary>
        public MessageInformation MessageInformation { get; set; }

        /// <summary>
        /// Container for SMTP related information.
        /// </summary>
        public SMTPInformation SMTPInformation { get; set; }
    }

    /// <summary>
    /// This will hold information where the e-mail will be sent to.
    /// </summary>
    public class RecipientInformation
    {
        /// <summary>
        /// The e-mail address of the recipient.
        /// </summary>
        public string ToEmail { get; set; }
    }

    /// <summary>
    /// This will be used to hold the sender's information.
    /// </summary>
    public class SenderInformation
    {
        /// <summary>
        /// The e-mail address of the sender.
        /// </summary>
        public string FromEmail { get; set; }

        /// <summary>
        /// The name of the sender.
        /// </summary>
        public string FromName { get; set; }

        /// <summary>
        /// The password of the sender.
        /// </summary>
        public string FromPassword { get; set; }
    }
    /// <summary>
    /// This will contain other properties of the message
    /// </summary>
    public class MessageInformation
    {
        /// <summary>
        /// The body of the e-mail message.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// The subject of the e-mail message.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Flag to identify if the client wants the body to be formatted as HTML
        /// </summary>
        public bool IsHTMLBody { get; set; }

        /// <summary>
        /// List of attachments that will be sent along with the e-mail
        /// </summary>
        public List<string> Attachments { get; set; }

        /// <summary>
        /// Flag to identify if the client wants to compress the attachments.
        /// Setting this to TRUE will compress attachments into a ZIP file before
        /// sending to the recipient(s).
        /// </summary>
        public bool CompressAttachments { get; set; }

        /// <summary>
        /// The name of the ZIP file that will be used when CompressAttachments is set to TRUE, note that extention does not have to be specified.
        /// A random 4-digit file name preceeded by an underscore will be used when the file name was not specified (e.g. _1093.zip)
        /// </summary>
        public string CompressedAttachmentFileName { get; set;  }
    }

    /// <summary>
    /// This will contain the STMP settings provided by the client
    /// </summary>
    public class SMTPInformation
    {
        /// <summary>
        /// The address of the SMTP host.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// The port number that will be used to send e-mails. If not specified, we will use 25 EnableSSL is false, otherwise we will use 587.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Flags if we are going to use SSL for sending the e-mail
        /// </summary>
        public bool EnableSSL { get; set; }

        /// <summary>
        /// Flags if we are going to use the default network credential
        /// </summary>
        public bool UseDefaultCredentials { get; set; }

    }
}
