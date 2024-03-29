﻿using System;
using System.Diagnostics.CodeAnalysis;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using System.Threading.Tasks;
using MimeKit.Text;
using Promact.Trappist.DomainModel.ApplicationClasses.BasicSetup;

namespace Promact.Trappist.Utility.EmailServices
{
    [ExcludeFromCodeCoverage]
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

            MimeMessage emailMessage = new MimeMessage();
            emailMessage.From.Add(MailboxAddress.Parse(from));
            emailMessage.To.Add(MailboxAddress.Parse(to));
            emailMessage.Body = new TextPart(TextFormat.Html)
            {
                Text = body
            };
            emailMessage.Subject = subject;
            using var client = new SmtpClient();
            var canParse = Enum.TryParse(_emailSettings.ConnectionSecurityOption, out SecureSocketOptions securityType);
            if (canParse)
            {
                if (securityType == SecureSocketOptions.None)
                {
                    await client.ConnectAsync(_emailSettings.Server, _emailSettings.Port, false); //ssl false
                }
                else
                    await client.ConnectAsync(_emailSettings.Server, _emailSettings.Port, securityType);
            }
            client.AuthenticationMechanisms.Remove("PLAIN");
            await client.AuthenticateAsync(_emailSettings.UserName, _emailSettings.Password);
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
            return true;
        }
        #endregion
    }
}
