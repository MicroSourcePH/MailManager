using System;
using MSI_MailManager.Models;
using NUnit.Framework;

namespace MSI_Tester
{
   
    public class Tests
    {
        MSI_MailManager.MailManager mailManager;
        Email email;

        #region Test Data Initialization
        [SetUp]
        public void Setup()
        {
            mailManager = new MSI_MailManager.MailManager();
            email = new Email()
            {
                MessageInformation = new MessageInformation()
                {
                    Body = "TEST BODY",
                    IsHTMLBody = false,
                    Subject = "TEST SUBJECT"
                },
                RecipientInformation = new RecipientInformation()
                {
                    ToEmail = "hil.jacla@gmail.com",
                    ToName = "Hilario Jacla III"
                },
                SenderInformation = new SenderInformation()
                {
                    FromEmail = "offshoreconfie@gmail.com",
                    FromName = "Hilario Jacla III",
                    FromPassword = "7tfRPX-=",
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
            if (mailManager.IsEmailValid(emailAddress))
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
            if(mailManager.IsEmailValid(emailAddress))
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
            if(mailManager.IsEmailValid(emailAddress))
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
            if(mailManager.IsEmailValid(emailAddress))
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
            var emailResult = mailManager.SendEmail(emailClone);
            if (emailResult.ToUpper() == "MESSAGE HAS BEEN SENT.")
            {
                Console.WriteLine(emailResult);
                Assert.Pass();
            }
            Assert.Fail();
        }

        #region Email.RecipientInformation Tests
        /// <summary>
        /// Ensures that our code can handle when the recipient's e-mail address was not supplied
        /// </summary>
        [Test]
        public void Email_RecipientInformation_ToEmail_IsNull()
        {
            Email emailClone = email;
            emailClone.RecipientInformation = new RecipientInformation()
            {
                ToName = "Hilario J. Jacla III"
            };
            var emailResult = mailManager.SendEmail(emailClone);
            if (emailResult.ToUpper() == "THIS FIELD IS REQUIRED: TOEMAIL.")
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        /// <summary>
        /// Ensures that our code can handle when the recipient's name was not supplied
        /// </summary>
        [Test]
        public void Email_RecipientInformation_ToName_IsNull()
        {
            Email emailClone = email;
            emailClone.RecipientInformation = new RecipientInformation()
            {
                ToEmail = "hil.jacla@gmail.com"
            };
            var emailResult = mailManager.SendEmail(emailClone);
            if (emailResult.ToUpper() == "THIS FIELD IS REQUIRED: TONAME.")
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        /// <summary>
        /// Ensures that our code can handle when both recipient's name and e-mail was not provided by our client.
        /// </summary>
        [Test]
        public void Email_RecipientInformation_ToNameAndToEmail_IsNull()
        {
            Email emailClone = email;
            emailClone.RecipientInformation = new RecipientInformation();
            var emailResult = mailManager.SendEmail(emailClone);
            if(emailResult.ToUpper() == "THE FOLLOWING FIELDS ARE REQUIRED: TOEMAIL,TONAME.")
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
            var emailResult = mailManager.SendEmail(emailClone);
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
            var emailResult = mailManager.SendEmail(emailClone);
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
            var emailResult = mailManager.SendEmail(emailClone);
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
            var emailResult = mailManager.SendEmail(emailClone);
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
            var emailResult = mailManager.SendEmail(emailClone);
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
            var emailResult = mailManager.SendEmail(emailClone);
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
            MSI_Runner.Program p = new MSI_Runner.Program();
            Email emailClone = email;
            emailClone.MessageInformation = new MessageInformation()
            {
                Body = "TEST BODY",
                Subject = "TEST SUBJECT",
                Attachments = p.GetTestAttachments(5)
            };
            string result = mailManager.SendEmail(emailClone);
            if(result.ToUpper() == "MESSAGE HAS BEEN SENT.")
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
                Port = 0
            };
            string result = mailManager.SendEmail(emailClone);
            if (result.ToUpper() == "THIS FIELD IS REQUIRED: HOST.")
            {
                Assert.Pass();
            }
            Assert.Fail();
        }
        #endregion

        #endregion
    }
}
