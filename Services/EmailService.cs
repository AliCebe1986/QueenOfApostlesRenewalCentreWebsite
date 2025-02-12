using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace QueenOfApostlesRenewalCentre.Services
{
    public class EmailService
    {
        private readonly string _apiKey;
        private readonly string _fromEmail;
        private readonly string _fromName;

        public EmailService(IConfiguration configuration)
        {
            _apiKey = configuration["SendGrid:ApiKey"]
                ?? throw new ArgumentNullException("SendGrid:ApiKey configuration is missing");
            _fromEmail = configuration["SendGrid:FromEmail"]
                ?? throw new ArgumentNullException("SendGrid:FromEmail configuration is missing");
            _fromName = configuration["SendGrid:FromName"]
                ?? throw new ArgumentNullException("SendGrid:FromName configuration is missing");
        }

        public async Task SendEmailAsync(string toEmail, string subject, string plainTextContent, string htmlContent)
        {
            var client = new SendGridClient(_apiKey);
            var from = new EmailAddress(_fromEmail, _fromName);
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            await client.SendEmailAsync(msg);
        }
    }
}
