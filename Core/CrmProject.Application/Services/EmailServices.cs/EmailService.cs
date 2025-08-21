using MimeKit;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using CrmProject.Application.Interfaces;
using CrmProject.Application.Settings;
using MailKit.Security;


namespace CrmProject.Application.Services.EmailServices.cs
{
    public class EmailService : IEmailService
    {
       
            private readonly EmailSettings _emailSettings; 

            public EmailService(IOptions<EmailSettings> emailSettings)
            {
                _emailSettings = emailSettings.Value;
            }

            public async Task SendEmailAsync(string to, string subject, string body)
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_emailSettings.SmtpUser));
                email.To.Add(MailboxAddress.Parse(to));
                email.Subject = subject;
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
                await smtp.ConnectAsync(_emailSettings.SmtpHost, _emailSettings.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_emailSettings.SmtpUser, _emailSettings.SmtpPass);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
        }
    }
