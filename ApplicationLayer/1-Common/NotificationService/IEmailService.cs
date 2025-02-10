namespace ApplicationLayer.Common.NotificationService
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string to, string subject, string body);
    }
}