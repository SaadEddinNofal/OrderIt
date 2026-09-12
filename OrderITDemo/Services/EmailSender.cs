using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace OrderITDemo.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var fromMail = _configuration["EmailSettings:Username"];
            var fromPassword = _configuration["EmailSettings:Password"];
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var smtpPort = int.Parse(_configuration["EmailSettings:Port"] ?? "587");
            var enableSsl = bool.Parse(_configuration["EmailSettings:EnableSsl"] ?? "true");

            var message = new MailMessage
            {
                From = new MailAddress(fromMail),
                Subject = subject,
                Body = $"<html><body>{htmlMessage}</body></html>",
                IsBodyHtml = true,
            };
            message.To.Add(email);

            using (var smtpClient = new SmtpClient(smtpServer))
            {
                smtpClient.Port = smtpPort;
                smtpClient.Credentials = new NetworkCredential(fromMail, fromPassword);
                smtpClient.EnableSsl = enableSsl;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;

                await smtpClient.SendMailAsync(message);
            }
        }
    }
}