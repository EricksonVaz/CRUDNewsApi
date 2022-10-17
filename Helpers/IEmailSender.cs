namespace CRUDNewsApi.Helpers
{
    public interface IEmailSender
    {
        Task<bool> SendEmailAsync(string recipientEmail, string recipientFirstName, string htmlBody);
    }
}
