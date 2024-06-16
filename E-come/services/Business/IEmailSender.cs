namespace E_come.services.Business
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string mailTo, string subject, string body, IList<IFormFile> attachments = null);
    }
}
