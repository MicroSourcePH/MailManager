using System;
using System.Collections.Generic;
using System.Diagnostics;
using MSI_MailManager;
using MSI_MailManager.Models;
using MSI_Runner;
using NUnit.Framework;

namespace MSI_Tester
{
   
    public class Tests
    {
        Email email;

        #region Test Data Initialization
        [SetUp]
        public void Setup()
        {
            email = new Email()
            {
                MessageInformation = new MessageInformation()
                {
                    Body = "TEST BODY",
                    IsHTMLBody = false,
                    Subject = "TEST SUBJECT"
                },
                RecipientInformation = new List<RecipientInformation> { new RecipientInformation() { ToEmail = "hil.jacla@gmail.com"} },
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
            };
        }
        #endregion

        #region IsEmailValid() Tests
        /// <summary>
        /// Checks that the method correctly validates a valid e-mail address
        /// </summary>
        [Test]
        public void Email_IsValid()
        {
            string emailAddress = "hil.jacla@gmail.com";
            if (MailManager.IsEmailValid(emailAddress))
            {
                Assert.Pass();
            }
            Assert.Fail();
        }


        /// <summary>
        /// Ensures that the method does not allow e-amil address without @
        /// </summary>
        [Test]
        public void Email_IsInvalid_If_WithoutATSymbol()
        {
            string emailAddress = "hiljaclagmail.com";
            if(MailManager.IsEmailValid(emailAddress))
            {
                Assert.Fail();
            }
            Assert.Pass();
        }

        /// <summary>
        /// Ensures that method does not allow e-mail address without domain
        /// </summary>
        [Test]
        public void Email_IsInvalid_If_WithoutDomain()
        {
            string emailAddress = "hil.jacla@gmail";
            if(MailManager.IsEmailValid(emailAddress))
            {
                Assert.Fail();
            }
            Assert.Pass();
        }

        /// <summary>
        /// Ensures that the method does not allow empty e-mail address string
        /// </summary>
        [Test]
        public void Email_IsInvalid_Empty()
        {
            string emailAddress = "";
            if(MailManager.IsEmailValid(emailAddress))
            {
                Assert.Fail();
            }
            Assert.Pass();
        }
        #endregion

        #region SendEmail() Tests
        /// <summary>
        /// Ensures that our code behaves as expected when all the required parameter was supplied by the client.
        /// </summary>
        [Test]
        public void Email_MessageInformation_DataIsComplete()
        {
            Email emailClone = email;
            var emailResult = MailManager.SendEmail(emailClone);
            Console.WriteLine(emailResult);
            if (emailResult.ToUpper() == "MESSAGE HAS BEEN SENT.")
            {
                Console.WriteLine(emailResult);
                Assert.Pass();
            }
            Assert.Fail();
        }

        #region Email.RecipientInformation Tests
        /// <summary>
        /// Ensures that our code can handle when no recipient was specified.
        /// </summary>
        [Test]
        public void Email_RecipientInformation_ToNameAndToEmail_IsNull()
        {
            Email emailClone = email;
            emailClone.RecipientInformation = new List<RecipientInformation>();
            var emailResult = MailManager.SendEmail(emailClone);
            Console.WriteLine(emailResult);
            if (emailResult.ToUpper() == "PLEASE SPECIFY AT LEAST 1 RECIPIENT.")
            {
                Assert.Pass();
            }
            Assert.Fail();
        }
        #endregion

        #region Email.SenderInformation Tests
        /// <summary>
        /// Ensures that our code can handle when the sender's e-mail address was not supplied
        /// </summary>
        [Test]
        public void Email_SenderInformation_FromEmail_IsNull()
        {
            Email emailClone = email;
            emailClone.SenderInformation = new SenderInformation()
            {
                FromName = "Hilario Jacla III",
                FromPassword = "Password1"
            };
            var emailResult = MailManager.SendEmail(emailClone);
            Console.WriteLine(emailResult);
            if (emailResult.ToUpper() == "THIS FIELD IS REQUIRED: FROMEMAIL.")
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        /// <summary>
        /// Ensures that our code can handle whent the sender's name was not supplied
        /// </summary>
        [Test]
        public void Email_SenderInformation_FromName_IsNull()
        {
            Email emailClone = email;
            emailClone.SenderInformation = new SenderInformation()
            {
                FromEmail = "offshoreconfie@gmail.com",
                FromPassword = "Password1"
            };
            var emailResult = MailManager.SendEmail(emailClone);
            Console.WriteLine(emailResult);
            if (emailResult.ToUpper() == "THIS FIELD IS REQUIRED: FROMNAME.")
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        /// <summary>
        /// Ensures that our code can handle when the sender's password was not supplied
        /// </summary>
        [Test]
        public void Email_SenderInformation_FromPassword_IsNull()
        {
            Email emailClone = email;
            emailClone.SenderInformation = new SenderInformation()
            {
                FromEmail = "offshoreconfie@gmail.com",
                FromName = "Hilario Jacla III"
            };
            var emailResult = MailManager.SendEmail(emailClone);
            Console.WriteLine(emailResult);
            if (emailResult.ToUpper() == "THIS FIELD IS REQUIRED: FROMPASSWORD.")
            {
                Assert.Pass();
            }
            Assert.Fail();
        }
        #endregion

        #region Email.MessageInformation Tests
        /// <summary>
        /// Ensures that the body of the email was supplied by the client.
        /// </summary>
        [Test]
        public void Email_MessageInformation_Body_IsNull()
        {
            Email emailClone = email;
            emailClone.MessageInformation = new MessageInformation()
            {
                //Body = "",
                IsHTMLBody = false,
                Subject = "TEST SUBJECT"
            };
            var emailResult = MailManager.SendEmail(emailClone);
            Console.WriteLine(emailResult);
            if (emailResult.ToUpper() == "THIS FIELD IS REQUIRED: BODY.")
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        /// <summary>
        /// Ensures that the subject of the email as supplied by the client
        /// </summary>
        [Test]
        public void Email_MessageInformation_Subject_IsNull()
        {
            Email emailClone = email;
            emailClone.MessageInformation = new MessageInformation()
            {
                Body = "TEST BODY",
                IsHTMLBody = false,
                //Subject = "TEST SUBJECT"
            };
            var emailResult = MailManager.SendEmail(emailClone);
            Console.WriteLine(emailResult);
            if (emailResult.ToUpper() == "THIS FIELD IS REQUIRED: SUBJECT.")
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        /// <summary>
        /// Ensures that the the code can handle if both subject and body was not supplied by the client.
        /// </summary>
        [Test]
        public void Email_MessageInformation_SubjectAndBody_IsNull()
        {
            Email emailClone = email;
            emailClone.MessageInformation = new MessageInformation()
            {
                //Body = "TEST BODY",
                IsHTMLBody = false,
                //Subject = "TEST SUBJECT"
            };
            var emailResult = MailManager.SendEmail(emailClone);
            Console.WriteLine(emailResult);
            if (emailResult.ToUpper() == "THE FOLLOWING FIELDS ARE REQUIRED: BODY,SUBJECT.")
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        /// <summary>
        /// Ensures that our code can send e-mails with attachments
        /// </summary>
        [Test]
        public void Email_MessageInformation_WithAttachments()
        {
         
            Program p = new Program();
            Email emailClone = email;
            emailClone.MessageInformation = new MessageInformation()
            {
                Body = "TEST BODY",
                Subject = "TEST SUBJECT",
                Attachments = p.GetTestAttachments(5)
            };
            string emailResult = MailManager.SendEmail(emailClone);
            Console.WriteLine(emailResult);
            if (emailResult.ToUpper() == "MESSAGE HAS BEEN SENT.")
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        /// <summary>
        /// Ensures that our code can handle when the client wants the attachment to be compressed and a file name was specified
        /// </summary>
        [Test]
        public void Email_MessageInformation_WithCompressedAttachments_ZipNameSpecified()
        {
            Program p = new Program();
            Email emailClone = email;
            emailClone.MessageInformation = new MessageInformation()
            {
                Body = "TEST BODY",
                Subject = "TEST SUBJECT",
                Attachments = p.GetTestAttachments(5),
                CompressAttachments=true,
                CompressedAttachmentFileName= "TESTCOMPRESSEDATTACHMENT"
            };
            string emailResult = MailManager.SendEmail(emailClone);
            Console.WriteLine(emailResult);
            if (emailResult.ToUpper() == "MESSAGE HAS BEEN SENT.")
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        /// <summary>
        /// Ensures that our code can handle when the client wants the attachment to be compressed and a file name was NOT specified
        /// </summary>
        [Test]
        public void Email_MessageInformation_WithCompressedAttachments_ZipNameNotSpecified()
        {
            Program p = new Program();
            Email emailClone = email;
            emailClone.MessageInformation = new MessageInformation()
            {
                Body = "TEST BODY",
                Subject = "TEST SUBJECT",
                Attachments = p.GetTestAttachments(5),
                CompressAttachments = true
            };
            string emailResult = MailManager.SendEmail(emailClone);
            Console.WriteLine(emailResult);
            if (emailResult.ToUpper() == "MESSAGE HAS BEEN SENT.")
            {
                Assert.Pass();
            }
            Assert.Fail();
        }
        #endregion

        #region Email.SMTPInformation Tests

        /// <summary>
        /// Ensures that our code can handle when the STMP Host URL was not provided by the client
        /// </summary>
        [Test]
        public void Email_SMTPInformation_Host_IsNull()
        {
            Email emailClone = email;
            email.SMTPInformation = new SMTPInformation()
            {
                EnableSSL = true,
                //Host = "smtp.gmail.com",
                //Port = 0,
                UseDefaultCredentials = true
            };
            string emailResult = MailManager.SendEmail(emailClone);
            Console.WriteLine(emailResult);
            if (emailResult.ToUpper() == "THIS FIELD IS REQUIRED: HOST.")
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void Email_SMTPInformtion_Host_IsInvalid()
        {
            Email emailClone = email;
            email.SMTPInformation = new SMTPInformation()
            {
                Host = "smtp.invalidhost.com",
                //Port = 587,
                EnableSSL = true,
                UseDefaultCredentials = true
            };
            string emailResult = MailManager.SendEmail(emailClone);
            Console.WriteLine(emailResult);
            if(emailResult.ToUpper() == "FAILURE SENDING MAIL. MAKE SURE THAT THE SMTP HOST AND PORT IS CORRECT.")
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        //TODO: Fix an issue where an exception is being thrown for this case: Port is not provided.
        //TODO: Test when host name is invalid
        #endregion

        #endregion
    }
}
