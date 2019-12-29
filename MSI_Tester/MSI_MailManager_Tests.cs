using MSI_MailManager.Models;
using NUnit.Framework;

namespace MSI_Tester
{
   
    public class Tests
    {
        MSI_MailManager.MailManager mailManager;
        Email email;

        [SetUp]
        public void Setup()
        {
            mailManager = new MSI_MailManager.MailManager();
            email = new Email()
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
                    FromEmail = "hil.jacla@gmail.com",
                    FromName = "Hilario Jacla III",
                    FromPassword = "qM.oB!L]-&pwM)zgL",
                },
                SMTPInformation = new MSI_MailManager.Models.SMTPInformation()
                {
                    EnableSSL = true,
                    Host = "smtp.gmail.com",
                    Port = 587,
                    UseDefaultCredentials = true
                }
            };
        }

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
        #region IsEmailValid() Tests
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
                isHTMLBody = false,
                Subject = "TEST SUBJECT"
            };
            var emailResult = mailManager.SendEmail(emailClone);
            if(emailResult.ToUpper() ==  "THIS FIELD IS REQUIRED: BODY.")
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
                isHTMLBody = false,
                //Subject = "TEST SUBJECT"
            };
            var emailResult = mailManager.SendEmail(emailClone);
            if (emailResult.ToUpper() == "THIS FIELD IS REQUIRED: SUBJECT.")
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        //Ensures that the the code can handle if both subject and body was not supplied by the client.
        [Test]
        public void Email_MessageInformation_SubjectAndBody_IsNull()
        {
            Email emailClone = email;
            emailClone.MessageInformation = new MessageInformation()
            {
                //Body = "TEST BODY",
                isHTMLBody = false,
                //Subject = "TEST SUBJECT"
            };
            var emailResult = mailManager.SendEmail(emailClone);
            if (emailResult.ToUpper() == "THE FOLLOWING FIELDS ARE REQUIRED: BODY,SUBJECT.")
            {
                Assert.Pass();
            }
            Assert.Fail();
        }
        #endregion
        #endregion
    }
}