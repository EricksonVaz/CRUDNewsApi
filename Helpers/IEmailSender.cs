namespace CRUDNewsApi.Helpers
{
    public interface IEmailSender
    {
        Task<bool> SendEmailAsync(string recipientEmail, string subject, string htmlBody);
    }
}
