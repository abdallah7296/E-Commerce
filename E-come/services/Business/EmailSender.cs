using E_come.DTO.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net;

namespace E_come.services.Business
{
    public class EmailSender : IEmailSender
    {
        private readonly IOptions<MailSettings> mailSettings;

        public EmailSender(IOptions<MailSettings> mailSettings)
        {
            this.mailSettings = mailSettings;
        }
        public async Task SendEmailAsync(string mailTo, string subject, string body, IList<IFormFile> attachments = null)
        {

            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(mailSettings.Value.Email),
                Subject = subject
            };
            email.To.Add(MailboxAddress.Parse(mailTo));
            var builder = new BodyBuilder();
            if (attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in attachments)
                {
                    if (file.Length > 0)
                    {
                        using var ms = new MemoryStream();
                        file.CopyTo(ms);
                        fileBytes = ms.ToArray();
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = body;
            email.Body = builder.ToMessageBody();
            email.From.Add(new MailboxAddress(mailSettings.Value.DisplayName, mailSettings.Value.Email));

            using var smtp = new SmtpClient();
            smtp.Connect(mailSettings.Value.Host, mailSettings.Value.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(mailSettings.Value.Email, mailSettings.Value.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
