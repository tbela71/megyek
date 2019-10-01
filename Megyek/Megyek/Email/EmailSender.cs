using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace Megyek.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;

        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                var mimeMessage = new MimeMessage()
                {
                    Subject = subject,
                    Body = new TextPart("html")
                    {
                        Text = message
                    }
                };
                mimeMessage.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.Sender));
                mimeMessage.To.Add(new MailboxAddress(email));

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
