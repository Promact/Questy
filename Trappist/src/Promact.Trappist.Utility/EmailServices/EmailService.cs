﻿using System;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using System.Threading.Tasks;
using MimeKit.Text;

namespace Promact.Trappist.Utility.EmailServices
{
    public class EmailService : IEmailService
    {
        #region IEmailService Method
        /// <summary>
        /// This method used for sending mail to client
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="server"></param>
        /// <param name="port"></param>
        /// <param name="connectionSecurityType"></param>
        /// <param name="body"></param>
        /// <param name="to"></param>
        /// <returns>It return true if mail sent else return false.</returns>
        public async Task<bool> SendMail(string userName, string password, string server, int port, string connectionSecurityType, string body, string to)
        {
            try
            {
                MimeMessage emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress(userName));
                emailMessage.To.Add(new MailboxAddress(to));
                emailMessage.Body = new TextPart(TextFormat.Html)
                {
                    Text = body
                };
                using (var client = new SmtpClient())
                {
                    var canParse = Enum.TryParse(connectionSecurityType, out SecureSocketOptions securityType);
                    if (canParse)
                    {
                        if (securityType == SecureSocketOptions.None)
                        {
                            //accept all SSL certificates (in case the server supports STARTTLS)
                            client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                            await client.ConnectAsync(server, port, false); //ssl false
                        }
                        else
                            await client.ConnectAsync(server, port, securityType);
                    }
                    await client.AuthenticateAsync(userName, password);
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
