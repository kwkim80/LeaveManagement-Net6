using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;

namespace LeaveManagement.Web.Services
{
    public class EmailSender : IEmailSender
    {
        private  string smtpServer;
        private  int smtpPort;
        private  string fromEmail;

        public EmailSender(string smtpServer, int smtpPort, string fromEmail)
        {
            this.smtpServer = smtpServer;
            this.smtpPort = smtpPort;
            this.fromEmail = fromEmail;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MailMessage
            {
                From = new MailAddress(fromEmail),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };

            message.To.Add(new MailAddress(email));

            //using (var client = new SmtpClient(smtpServer, smtpPort))
            //{
            //    client.Send(message);
            //}

            using var client = new SmtpClient(smtpServer, smtpPort);
            client.Send(message);

            return Task.CompletedTask;
        }
    }
}
