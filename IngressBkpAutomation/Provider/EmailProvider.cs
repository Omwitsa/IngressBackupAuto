using IngressBkpAutomation.Constants;
using IngressBkpAutomation.IProvider;
using IngressBkpAutomation.Utilities;
using IngressBkpAutomation.ViewModel;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace IngressBkpAutomation.Provider
{
    public class EmailProvider : IEmailProvider
    {
        public List<EmailMessage> ReceiveEmail(int maxCount = 10)
        {
            throw new NotImplementedException();
        }

        public async Task<ReturnData<bool>> SendEmailAsync(EmailMessage emailMessage, MailSettings smtpSettings)
        {
            var email = new MimeMessage();
            email.To.AddRange(emailMessage.ToAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
            email.From.AddRange(emailMessage.FromAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
            email.Subject = emailMessage.Subject;

            var builder = new BodyBuilder();
            //var image = builder.LinkedResources.Add(emailMessage.InstitutionLogo);
            //image.ContentId = "logoId";
            foreach (var file in emailMessage.Attachments)
                builder.Attachments.Add(file);

            builder.HtmlBody = emailMessage.Body;
            email.Body = builder.ToMessageBody();

            var options = SecureSocketOptions.StartTls;
            if (smtpSettings.SocketOption.ToUpper().Equals(StrValues.SSL))
                options = SecureSocketOptions.SslOnConnect;
            if (smtpSettings.SocketOption.ToLower().Equals(StrValues.NONE))
                options = SecureSocketOptions.None;

            try
            {
                using var smtp = new SmtpClient();
                smtp.Connect(smtpSettings.Server, smtpSettings.Port, options);
                smtp.Authenticate(smtpSettings.EmailId, Decryptor.Decrypt(smtpSettings.Password));
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
                return new ReturnData<bool>
                {
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                return new ReturnData<bool>
                {
                    Success = false,
                    Message = "Sorry, An error occurred while sending email"
                };
            }
        }
    }
}
