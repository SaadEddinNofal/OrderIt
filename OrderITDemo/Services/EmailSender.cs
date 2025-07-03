using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace OrderITDemo.Services
{
    public class EmailSender :  IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var fromMail = "sarg10nofal10@gmail.com";
            var fromPassword = "ebhqwsumzrqjkejq"; // من الأفضل استخدام كلمة مرور للتطبيق هنا  
            var message = new MailMessage
            {
                From = new MailAddress(fromMail),
                Subject = subject,
                Body = $"<html><body>{htmlMessage}</body></html>",
                IsBodyHtml = true,
                            };
            message.To.Add(email);

            using (var smtpClient = new SmtpClient("smtp.gmail.com")) // تأكد من أن هذا هو خادم SMTP الصحيح  
            {
                smtpClient.Port = 587; // المنفذ المناسب  
                smtpClient.Credentials = new NetworkCredential(fromMail, fromPassword);
                smtpClient.EnableSsl = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;

                await smtpClient.SendMailAsync(message); // استخدم SendMailAsync  
            }
        }
        /*using System;
        using System.Threading.Tasks;
        using MailKit.Net.Smtp;
        using MimeKit;*/

        /*public class EmailSender
        {
            public async Task SendEmailAsync(string email, string subject, string htmlMessage)
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Your Name", "your_email@gmail.com")); // وضع اسمك وعنوان بريدك الإلكتروني هنا  
                message.To.Add(new MailboxAddress("", email)); // البريد الإلكتروني المستلم  
                message.Subject = subject;

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = htmlMessage
                };
                message.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    try
                    {
                        // الاتصال بخادم SMTP Gmail  
                        await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);

                        // استخدام اسم المستخدم وكلمة المرور الخاصة بك  
                        await client.AuthenticateAsync("sarg10nofal10@gmail.com", "your_app_passwor");

                        // إرسال الرسالة  
                        await client.SendAsync(message);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error occurred while sending email: " + ex.Message);
                    }
                    finally
                    {
                        // قطع الاتصال بالخادم  
                        await client.DisconnectAsync(true);
                    }
                }
            }
        }*/
    }
}
