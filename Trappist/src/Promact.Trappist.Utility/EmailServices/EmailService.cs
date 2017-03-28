using System;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using System.Threading.Tasks;
using MimeKit.Text;
using Promact.Trappist.DomainModel.ApplicationClasses.BasicSetup;

namespace Promact.Trappist.Utility.EmailServices
{
    public class EmailService : IEmailService
    {
        #region Private Variables
        #region Dependencies
        private readonly EmailSettings _emailSettings;
        #endregion
        #endregion

        #region Constructor
        public EmailService(EmailSettings emailSettings)
        {
            _emailSettings = emailSettings;
        }
        #endregion

        #region IEmailService Method
        public async Task<bool> SendMailAsync(string to, string from, string body, string subject)
        {
            try
            {
                MimeMessage emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress(from));
                emailMessage.To.Add(new MailboxAddress(to));
                emailMessage.Body = new TextPart(TextFormat.Html)
                {
                    Text = body
                };
                emailMessage.Subject = subject;
                using (var client = new SmtpClient())
                {
                    var canParse = Enum.TryParse(_emailSettings.ConnectionSecurityOption, out SecureSocketOptions securityType);
                    if (canParse)
                    {
                        if (securityType == SecureSocketOptions.None)
                        {
                            //accept all SSL certificates (in case the server supports STARTTLS)
                            client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                            await client.ConnectAsync(_emailSettings.Server, _emailSettings.Port, false); //ssl false
                        }
                        else
                            await client.ConnectAsync(_emailSettings.Server, _emailSettings.Port, securityType);
                    }
                    await client.AuthenticateAsync(_emailSettings.UserName, _emailSettings.Password);
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                }
                return true;
            }
            catch (Exception)
            {
                return false; //issue in sending mail
            }
        }
        #endregion
    }
}
