using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using Org.BouncyCastle.Crypto.Macs;
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
            // Create message
            var toEmail = new MimeMessage();
            //toEmail.Sender = MailboxAddress.Parse("dotnetproject540@gmail.com");
            //toEmail.Sender.Name = "Shopping Basket's Developer";
            toEmail.From.Add(MailboxAddress.Parse("dotnetproject540@gmail.com"));
            toEmail.To.Add(MailboxAddress.Parse(email));
            toEmail.Subject = subject;
            toEmail.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = htmlMessage
            };

            // send email
            using (var smtpClient = new MailKit.Net.Smtp.SmtpClient())
            {
                smtpClient.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                smtpClient.Authenticate("dotnetproject540@gmail.com", "ouyrxlbzcmcrossv");
                smtpClient.Send(toEmail);
                smtpClient.Disconnect(true);
            }
            return Task.CompletedTask;
        }
    }
}
