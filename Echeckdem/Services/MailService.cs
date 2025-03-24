using DocumentFormat.OpenXml.Wordprocessing;
using System.Net.Mail;
using System.Net;

namespace Echeckdem.Services
{
    public class MailService
    {
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _emailSender;
        private readonly string _emailPassword;

        public MailService(IConfiguration configuration)
        {
            _smtpHost = configuration["EmailSettings:SmtpHost"];
            _smtpPort = int.Parse(configuration["EmailSettings:SmtpPort"]);
            _emailSender = configuration["EmailSettings:EmailSender"];
            _emailPassword = configuration["EmailSettings:EmailPassword"];
        }

        public async Task<bool> SendEmailAsync (string recipient, string subject, string body)
        {
            try
            {
                using (var smtp = new SmtpClient(_smtpHost, _smtpPort))
                {
                    smtp.Credentials = new NetworkCredential(_emailSender, _emailPassword);
                    smtp.EnableSsl = true;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_emailSender),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    };
                    mailMessage.To.Add(recipient);

                    await smtp.SendMailAsync(mailMessage);
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Log error (optional)
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }
        }
    }


}

