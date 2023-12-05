using IngressBkpAutomation.ViewModel;

namespace IngressBkpAutomation.IProvider
{
    public interface IEmailProvider
    {
        Task<ReturnData<bool>> SendEmailAsync(EmailMessage message, MailSettings smtpSettings);
        List<EmailMessage> ReceiveEmail(int maxCount = 10);
    }
}
