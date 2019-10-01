using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Megyek.Email
{
    public class EmailsSender : IEmailsSender
    {
        private readonly EmailSettings _emailSettings;

        public EmailsSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailsAsync(List<string> emails, string subject, string htmlMessage)
        {
            try
            {
                var mimeMessage = new MimeMessage()
                {
                    Subject = subject,
                    Body = new TextPart("html")
                    {
                        Text = htmlMessage
                    }
                };
                mimeMessage.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.Sender));
                foreach (string email in emails)
                {
                    mimeMessage.Bcc.Add(new MailboxAddress(email));
                }

                SmtpClient client = new SmtpClient
                {
                    ServerCertificateValidationCallback = (s, c, h, e) => true
                };
                await client.ConnectAsync(_emailSettings.MailServer);
                await client.AuthenticateAsync(_emailSettings.Sender, _emailSettings.Password);
                await client.SendAsync(mimeMessage);
                await client.DisconnectAsync(true);
                client.Dispose();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}
