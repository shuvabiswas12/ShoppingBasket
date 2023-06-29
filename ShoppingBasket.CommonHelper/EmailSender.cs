using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasket.CommonHelper
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var toEmail = new MimeMessage();
            toEmail.From.Add(MailboxAddress.Parse("AppGmailId"));
            toEmail.To.Add(MailboxAddress.Parse(email));
            toEmail.Subject = subject;
            toEmail.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = htmlMessage
            };
            using (var smtpClient = new MailKit.Net.Smtp.SmtpClient())
            {
                smtpClient.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                smtpClient.Authenticate("UserId", "Password");
                smtpClient.SendAsync(toEmail);
                smtpClient.Disconnect(true);
            }
            return Task.CompletedTask;
        }
    }
}
